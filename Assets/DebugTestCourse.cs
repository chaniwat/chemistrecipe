using ChemistRecipe.Experiment;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebugTestCourse : MonoBehaviour {

    public CourseBehaviour courseBehaviour;

    private Text text;

	// Use this for initialization
	void Start ()
    {
        text = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        string totalText = "";
        string[] equipmentNames = new string[] { "Beaker_Water", "Plate_Sodium_Hydroxide", "Bottle_Coconut_Oil" };

        FillableEquipment equipment;
        foreach (string name in equipmentNames)
        {
            equipment = (FillableEquipment) courseBehaviour.CourseScript.GetEquipmentByObjectName(name);

            totalText += equipment.name + "'s Material list:\n";

            foreach (KeyValuePair<ChemistRecipe.Experiment.Material, Volume> pair in equipment.Materials)
            {
                totalText += pair.Key.name + ": Type => " + pair.Key.type.ToString() + " | Volume: " + pair.Value.volume + pair.Value.metric.ToString() + ", Temp: " + pair.Value.tempature + "\n";
            }

            totalText += "\n\n";
        }

        text.text = totalText;
	}
}
