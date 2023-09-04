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
    private int DotAmount = 10;
    private Scene simulationScene;
    private PhysicsScene2D physicsScene;
    private Dictionary<Transform, Transform> moveableObjects = new Dictionary<Transform, Transform>();
    private List<LaunchPad> launchpads;
    private List<GravityWell> GravityWells;
    private bool show = false;
    private void Start()
    {
        //if (SceneUtility.GetBuildIndexByScenePath("Simulation") != -1)
        //{
            DotAmount++;
            CreatePhysicsScene();
            CreateDotList();
            Hide();
        //}
        int num = 0;
        launchpads = new List<LaunchPad>();
        foreach (var obj in moveableObjects)
        {
            num++;
            obj.Value.position = obj.Key.position;
            obj.Value.rotation = obj.Key.rotation;
            int c = obj.Value.childCount;
            for (int i = 0; i < c; i++)
            {
                obj.Value.GetChild(i).position = obj.Key.GetChild(i).position;
                obj.Value.GetChild(i).rotation = obj.Key.GetChild(i).rotation;
                if (obj.Value.GetChild(i).TryGetComponent<LaunchPad>(out LaunchPad LP))
                {
                    launchpads.Add(LP);
                }
            }
        }
    }
    void CreatePhysicsScene()
    {
        if (gameObject.scene.name == "Simulation") return;
        simulationScene = SceneManager.CreateScene("Simulation", new CreateSceneParameters(LocalPhysicsMode.Physics2D));
        physicsScene = simulationScene.GetPhysicsScene2D();
        GravityWells = new List<GravityWell>();
        foreach (Transform obj in objectsParent)
        {
            var ghostObj = Instantiate(obj.gameObject, obj.position, obj.rotation);
            if(ghostObj.TryGetComponent<Renderer>(out Renderer ren))
            {
                ren.enabled = false;
            }
            Renderer[] renderers = ghostObj.GetComponentsInChildren<Renderer>();

            foreach (Renderer cRen in renderers)
                cRen.enabled = false;
            SceneManager.MoveGameObjectToScene(ghostObj, simulationScene);
            if (ghostObj.CompareTag("Movable")) moveableObjects.Add(obj, ghostObj.transform);
        }
        GravityWells = new List<GravityWell>(GameObject.FindObjectsByType<GravityWell>(FindObjectsSortMode.InstanceID));
        for(int i = GravityWells.Count - 1; i >= 0; i--)
        {
            if (GravityWells[i].gameObject.scene.name != "Simulation")
                GravityWells.RemoveAt(i);
        }
    }
    private void FixedUpdate()
    {

    }
    [SerializeField] LineRenderer line;
    [SerializeField] int MaxPhysicsFrameIterations;
    public void SimulatrTrajectory(Vector2 pos, Vector2 force, Vector2 currentMvmt, float currentRot, Quaternion transformRot)
    {
        var ghostObj = Instantiate(Ball.gameObject, pos, transformRot);
        //ghostObj.GetComponent<BallSpeedReporter>().isReal = false;
        SceneManager.MoveGameObjectToScene(ghostObj.gameObject, simulationScene);
        Collider2D ghostCol = ghostObj.GetComponent<Collider2D>();
        BallGravityhandler ghostGrav = ghostObj.GetComponent<BallGravityhandler>();
        
        //set gravity wells to use affected object
        //for(int i = 0; i < GravityWells.Count; i++)
        //{
        //    //GravityWells[i].SetAffectEdObjectListForProjection(ghostObj);
        //}
        //set gravity wells to use affected object

        if (ghostObj.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {
            rb.velocity = rb.velocity + currentMvmt;
            rb.rotation = currentRot;
            //rb.AddForce(force, ForceMode2D.Impulse);
            rb.velocity = rb.velocity + force;
        }
        Vector2[] LinePos = new Vector2[MaxPhysicsFrameIterations];
        line.positionCount = MaxPhysicsFrameIterations;
        line.SetPosition(0, ghostObj.transform.position);
        for (int i = 1; i < MaxPhysicsFrameIterations; i++)
        {
            if (true)
            {
                physicsScene.Simulate(Time.fixedDeltaTime);
                Vector3 sPos = new Vector3(ghostObj.transform.position.x, ghostObj.transform.position.y, ghostObj.transform.position.z + 11);
                for (int j = 0; j < GravityWells.Count; j++)
                {
                    if (i > -1)
                        GravityWells[j].AddGravToObject(ghostObj, ghostCol, ghostGrav, 1.0f);
                }
                for (int j = 0; j < launchpads.Count; j++)
                {
                    launchpads[j].GhostUpdate();
                }
                line.SetPosition(i, sPos);
                LinePos[i] = sPos;
                //for (int j = 0; j < GravityWells.Count; j++)
                //{
                //    GravityWells[j].AddGravToObject(ghostObj, ghostCol, ghostGrav, 0.4f);
                //}
            }
        }
        //SetDotPositions(LinePos);
        //DebugText.text = ghostObj.GetComponent<CollisionCounter>().Collisions.ToString();
        Destroy(ghostObj);
    }
    private void SetDotPositions(Vector2[] LinePos)
    {
        float totalChangeOfPos = 0;
        for(int i = 1; i < MaxPhysicsFrameIterations; i++)
        {
            totalChangeOfPos += (LinePos[i] - LinePos[i - 1]).magnitude;
        }
        totalChangeOfPos /= DotAmount;
        int currDotIndex = 0;
        float currChangeInPos = 0;
        for (int i = 1; i < MaxPhysicsFrameIterations; i++)
        {
            currChangeInPos += (LinePos[i] - LinePos[i - 1]).magnitude;
            if(currChangeInPos >= totalChangeOfPos)
            {
                currChangeInPos = 0;
                DotList[currDotIndex].transform.position = new Vector3(LinePos[i].x, LinePos[i].y, Ball.transform.position.z);
                currDotIndex++;
            }
        }
    }
    public void Show()
    {
        line.enabled = true;
        //for (int i = 0; i < DotList.Count - 1; i++)
        //{
        //    if (DotList[i].TryGetComponent<Renderer>(out Renderer ren))
        //    {
        //        DebugText.text = DotList.Count.ToString();
        //        ren.enabled = true;
        //    }
        //}
    }
    public void Hide()
    {
        line.enabled = false;
        for (int i = 0; i < DotList.Count - 1; i++)
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
            //SceneManager.MoveGameObjectToScene(dot, simulationScene);
            if(dot.TryGetComponent<Renderer>(out Renderer ren))
            {
                ren.enabled = false;
            }
            float scale = (dot.transform.localScale.x + 10f) / (float)(i + 10) / 4f;
            dot.transform.localScale = new Vector2(scale, scale);
            DotList.Add(dot);
        }
    }
}
