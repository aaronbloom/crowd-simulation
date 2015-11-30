using UnityEngine;

public class UserInterfaceController : MonoBehaviour {

    private GameObject mainMenu;
    private GameObject setupMenu;
    private GameObject environmentBuilderMenu;
    private BootStrapper bootStrapper;
    private Object userEnvironmentBuilder;

    void Awake() {
        mainMenu = GameObject.Find("MainMenu");
        setupMenu = GameObject.Find("SetupMenu");
        environmentBuilderMenu = GameObject.Find("EnvironmentBuilderMenu");
    }

	void Start () {
	    mainMenu.SetActive(true);
        setupMenu.SetActive(false);
        environmentBuilderMenu.SetActive(false);
        bootStrapper = GameObject.Find("Bootstrapper").GetComponent<BootStrapper>();
    }
	
	void Update () {
	
	}

    public void NewSimulation() {
        mainMenu.SetActive(false);
        setupMenu.SetActive(true);
    }

    public void StartEnvironmentBuilder() {
        setupMenu.SetActive(false);
        environmentBuilderMenu.SetActive(true);
        userEnvironmentBuilder = MonoBehaviour.Instantiate(Resources.Load("Prefabs/UserEnvironmentBuilder"));
    }

    public void StartSimulation() {
        GameObject.Destroy(userEnvironmentBuilder);
        mainMenu.SetActive(false);
        setupMenu.SetActive(false);
        environmentBuilderMenu.SetActive(false);
        int numberOfBoids = setupMenu.GetComponent<SliderController>().Value;
        bootStrapper.StartSimulation(numberOfBoids);
    }
}
