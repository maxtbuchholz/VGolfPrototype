using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Projection : MonoBehaviour
{
    [SerializeField] Transform objectsParent;
    [SerializeField] GameObject Ball;
    [SerializeField] GameObject ProjectionDot;
    [SerializeField] private TextMeshProUGUI DebugText;
    private List<GameObject> DotList;
    [SerializeField] int DotAmount;
    private Scene simulationScene;
    private PhysicsScene2D physicsScene;
    private Dictionary<Transform, Transform> moveableObjects = new Dictionary<Transform, Transform>();
    private bool show = false;
    private void Start()
    {
        CreatePhysicsScene();
        CreateDotList();
        Hide();
    }
    void CreatePhysicsScene()
    {
        simulationScene = SceneManager.CreateScene("Simulation", new CreateSceneParameters(LocalPhysicsMode.Physics2D));
        physicsScene = simulationScene.GetPhysicsScene2D();

        foreach(Transform obj in objectsParent)
        {
            var ghostObj = Instantiate(obj.gameObject, obj.position, obj.rotation);
            if(ghostObj.TryGetComponent<Renderer>(out Renderer ren))
            {
                ren.enabled = false;
            }
            SceneManager.MoveGameObjectToScene(ghostObj, simulationScene);
            if (ghostObj.CompareTag("Movable")) moveableObjects.Add(obj, ghostObj.transform);

        }
    }
    private void Update()
    {
        int num = 0;
        foreach(var obj in moveableObjects)
        {
            num++;
            obj.Value.position = obj.Key.position;
            obj.Value.rotation = obj.Key.rotation;
        }
        DebugText.text = num.ToString();
    }
    [SerializeField] LineRenderer line;
    [SerializeField] int MaxPhysicsFrameIterations;
    public void SimulatrTrajectory(Vector2 pos, Vector2 force, Vector2 currentMvmt, float currentRot, Quaternion transformRot)
    {
        var ghostObj = Instantiate(Ball.gameObject, pos, transformRot);
        ghostObj.GetComponent<BallSpeedReporter>().isReal = false;
        SceneManager.MoveGameObjectToScene(ghostObj.gameObject, simulationScene);

        if(ghostObj.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {
            rb.AddForce(currentMvmt, ForceMode2D.Impulse);
            rb.rotation = currentRot;
            rb.AddForce(force, ForceMode2D.Impulse);
        }

        line.positionCount = MaxPhysicsFrameIterations;
        for (int i = 0; i < MaxPhysicsFrameIterations; i++)
        {
            physicsScene.Simulate(Time.fixedDeltaTime);
            line.SetPosition(i, ghostObj.transform.position);
            //if(i % (int)((float)(MaxPhysicsFrameIterations / (float)DotAmount)) == 0)DotList[(int)((float)i / ((float)MaxPhysicsFrameIterations / (float)DotAmount))].transform.position = ghostObj.transform.position;
        }
        Destroy(ghostObj);
    }
    public void Show()
    {
        line.enabled = true;
        for(int i = 1; i < DotList.Count; i++)
        {
            if (DotList[i].TryGetComponent<Renderer>(out Renderer ren))
            {
                //ren.enabled = true;
            }
        }
    }
    public void Hide()
    {
        line.enabled = false;
        for (int i = 0; i < DotList.Count; i++)
        {
            if (DotList[i].TryGetComponent<Renderer>(out Renderer ren))
            {
                ren.enabled = false;
            }
        }
    }
    public void CreateDotList()
    {
        DotList = new List<GameObject>();
        for(int i = 0; i < DotAmount; i++)
        {
            GameObject dot = Instantiate(ProjectionDot.gameObject, Vector2.zero, Quaternion.identity);
            SceneManager.MoveGameObjectToScene(dot, simulationScene);
            if(dot.TryGetComponent<Renderer>(out Renderer ren))
            {
                ren.enabled = false;
            }
            DotList.Add(dot);
        }
    }
}
