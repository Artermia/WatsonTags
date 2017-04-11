using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SymptomsTable : MonoBehaviour {

    public GameObject popup;

	public class Symptom
	{
		public string Title;
		public string Definition;

		//Ctor
		public Symptom() {}

		//Ctor
		public Symptom(string title)
		{
			Title = title;
		}
	}
		
	static Hashtable symptoms = new Hashtable();

    // Use this for initialization
    void Start()
    {
        //Create list of symptoms.
        //SymptomsTable appendix = new SymptomsTable();
        loadSymptoms();
        //Retrieve table
        //Debug.Log(appendix.symptoms);
    }

    // Update is called once per frame
    void Update() { }

    //Appendix Ctor
    public void loadSymptoms() {

		//changed from coughing
		symptoms ["Cough"] = "A rapid expulsion of air from the lungs, typically in order to clear the lung airways of fluids, mucus, or other material. Also known as tussis.";
		symptoms ["Fever"] = "Although a fever technically is any body temperature above the normal of 98.6 F (37 C), in practice a person is usually not considered to have a significant fever until the temperature is above 100.4 F (38 C).";
		symptoms ["Internal Bleeding"] = "Bleeding inside the body that is not seen from the outside. Internal bleeding occurs when damage to an artery or vein allows blood to escape the circulatory system and collect inside the body. The internal bleeding may occur within tissues, organs, or in cavities of the body including the head, chest, and abdomen.";
		symptoms ["Fatigue"] = "A condition characterized by a lessened capacity for work and reduced efficiency of accomplishment, usually accompanied by a feeling of weariness and tiredness. Fatigue can be acute and come on suddenly or chronic and persist.";
		symptoms ["Nausea"] = "Stomach queasiness, the urge to vomit.";
		symptoms ["Sweating"] = "The act of secreting fluid from the skin by the sweat (sudoriferous) glands. These are small tubular glands situated within and under the skin (in the subcutaneous tissue). They discharge by tiny openings in the surface of the skin.";
		symptoms ["Headache"] = "A pain in the head with the pain being above the eyes or the ears, behind the head (occipital), or in the back of the upper neck. Headache, like chest pain or back ache, has many causes.";
		symptoms ["Diarrhea"] = "A common condition that involves unusually frequent and liquid bowel movements. The opposite of constipation. There are many infectious and noninfectious causes of diarrhea.";

		//changed from stiff
		symptoms ["Stiff neck"] = "Also called torticollis or spasmodic torticollis, this is the most common of the focal dystonias: a state of abnormal -- either excessive of inadequate -- muscle tone."; 

		//this is a diagnosis
		symptoms ["Irritable"] = "Restless or more cranky than usual.";

		symptoms["Arm Pain"] = "Brachial neuritis: Inflammation of nerves in the arm causing muscle weakness and pain.";
		symptoms["Cramping Leg Pain"] = "An aching, crampy, tired, and sometimes burning pain in the legs that comes and goes; it typically occurs with walking and goes away with rest. Known medically as intermittent claudication.";
		symptoms["Back Pain"] = "Pain felt in the low or upper back. Causes of pain in the low and upper back include conditions affecting the bony spine; discs between the vertebrae; ligaments around the spine and discs; spinal inflammation; spinal cord and nerves; muscles; internal organs of the pelvis, chest, and abdomen; tumors; and the skin.";
		symptoms["Neck Pain"] = "Neck pain is commonly associated with dull aching. Sometimes pain in the neck is worsened with movement of the neck or turning the head. Other symptoms associated with some forms of neck pain include numbness, tingling, tenderness, sharp shooting pain, fullness, difficulty swallowing, pulsations, swishing sounds in the head, dizziness or lightheadedness, and lymph node (gland) swelling."; 
		symptoms["Discoloration"] = "Erythrocyanosis: Discoloration on the legs that has a bluish or purple hue.";
	}

    public void spawnSymptom(string input)
    {
        //Prevent multiple popups
        if(GameObject.FindGameObjectWithTag("Popup") != null)
        {
            return;
        }
        GameObject popupBox = Instantiate(popup);
        popupBox.transform.FindChild("Message").GetComponent<Text>().text = (string)symptoms[input];
        popupBox.transform.FindChild("Message").GetComponent<Text>().resizeTextForBestFit = true;

        GameObject user = GameObject.Find("HoloLensCamera");
        popupBox.transform.position = user.transform.position + user.transform.forward * 2;
        popupBox.transform.rotation = user.transform.rotation;
    }
}

