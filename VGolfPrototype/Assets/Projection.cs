using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Projection : MonoBehaviour
{
    [SerializeField] Transform objectsParent;
    [SerializeField] GameObject Ball;
    private Scene simulationScene;
    private PhysicsScene2D physicsScene;
    private Dictionary<Transform, Transform> moveableObjects = new Dictionary<Transform, Transform>();
    private bool show = false;
    private void Start()
    {
        Hide();
        CreatePhysicsScene(); 
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
            if (!ghostObj.isStatic) moveableObjects.Add(obj, ghostObj.transform);

        }
    }
    private void Update()
    {
        foreach(var obj in moveableObjects)
        {
            obj.Value.position = obj.Key.position;
            obj.Value.rotation = obj.Key.rotation;
        }
    }
    [SerializeField] LineRenderer line;
    [SerializeField] int MaxPhysicsFrameIterations;
    public void SimulatrTrajectory(Vector2 pos, Vector2 force)
    {
        var ghostObj = Instantiate(Ball.gameObject, pos, Quaternion.identity);
        SceneManager.MoveGameObjectToScene(ghostObj.gameObject, simulationScene);

        if(ghostObj.TryGetComponent<Rigidbody2D>(out Rigidbody2D rb))
        {
            rb.AddForce(force, ForceMode2D.Impulse);
        }

        line.positionCount = MaxPhysicsFrameIterations;
        for (int i = 0; i < MaxPhysicsFrameIterations; i++)
        {
            physicsScene.Simulate(Time.fixedDeltaTime);
            line.SetPosition(i, ghostObj.transform.position);
        }
        Destroy(ghostObj);
    }
    public void Show()
    {
        line.enabled = true;
    }
    public void Hide()
    {
        line.enabled = false;
    }
}
