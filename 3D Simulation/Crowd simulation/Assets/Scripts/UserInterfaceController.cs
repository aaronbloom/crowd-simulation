using UnityEngine;

public class UserInterfaceController : MonoBehaviour {

    private GameObject mainMenu;
    private GameObject setupMenu;
    private BootStrapper bootStrapper;

    void Awake() {
        mainMenu = GameObject.Find("MainMenu");
        setupMenu = GameObject.Find("SetupMenu");
    }

	void Start () {
	    mainMenu.SetActive(true);
        setupMenu.SetActive(false);
        bootStrapper = GameObject.Find("Bootstrapper").GetComponent<BootStrapper>();
    }
	
	void Update () {
	
	}

    public void NewSimulation() {
        mainMenu.SetActive(false);
        setupMenu.SetActive(true);
    }

    public void StartSimulation() {
        mainMenu.SetActive(false);
        setupMenu.SetActive(false);
        int numberOfBoids = setupMenu.GetComponent<SliderController>().Value;
        bootStrapper.StartSimulation(numberOfBoids);
    }
}
