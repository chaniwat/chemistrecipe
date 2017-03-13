using ChemistRecipe.Experiment;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;
using ChemistRecipe.AR;
using chemistrecipe.localization;

public class TestCourseScript : CourseScript
{
    // Final Animate
    public GameObject finalSoap;

    // Materials
    private const string WATER = "Water";
    private const string SODIUM_HYDROXIDE = "Sodium Hydroxide";
    private const string COCONUT_OIL = "Coconut Oil";
    private const string MIXED_WATER_SODIUM_HYDROXIDE = "Water + Sodium Hydroxide";
    private const string SOAP_LIQUID = "Soap (liquid)";

    // Checkpoint (Step)
    private bool FILL_SODIUM_HYDROXIDE_TO_WATER = false,
        MIX_SODIUM_HYDROXIDE_TO_WATER = false,
        FILL_MIXED_WATER_SODIUM_HYDROXIDE_TO_OIL = false,
        MIX_MIXED_WATER_SODIUM_HYDROXIDE_TO_OIL = false;

    // Material fill data
    private float accumulateFillSodiumHydroxide = 0f;
    private float accumulateFillCoconutOil = 0f;


    // Equipments
    private FillableEquipment beakerWater, plateSodiumHydroxide, bottleCoconutOil;

    protected override void SetupCoruse()
    {
        registerLocale();

        List<string> equipmentNames = new List<string>(new string[]{ "Beaker_Water", "Plate_Sodium_Hydroxide", "Bottle_Coconut_Oil" });
        
        foreach (FillableEquipment equipment in GetAllEquipment().Values)
        {
            if (!equipmentNames.Contains(equipment.name)) continue;

            equipment.OnBeforeUpdate = () =>
            {
                beforeUpdateEquipment(equipment);
            };
            equipment.OnStir = () =>
            {
                stirEquipment(equipment);
            };
            equipment.OnAfterFill = (material, volume) =>
            {
                afterFillEquipment(equipment, material, volume);
            };
        }

        beakerWater = (FillableEquipment)GetEquipmentByObjectName("Beaker_Water");
        plateSodiumHydroxide = (FillableEquipment)GetEquipmentByObjectName("Plate_Sodium_Hydroxide");
        bottleCoconutOil = (FillableEquipment)GetEquipmentByObjectName("Bottle_Coconut_Oil");
    }

    private void registerLocale()
    {
        LocalLanguage thLocale = courseBehaviour.globalObject.localization["th"];

        thLocale.setString("course.create_soap.instruction.1", "ทำการเทโซเดียมไฮดอรกไซด์ผสมกับน้ำ");
        thLocale.setString("course.create_soap.instruction.2", "ทำการคนให้สารผสมกัน");
        thLocale.setString("course.create_soap.instruction.3", "ทำการเทน้ำมันมะพร้าว");
        thLocale.setString("course.create_soap.instruction.4", "ทำการคนให้สารผสมกัน");
        thLocale.setString("course.create_soap.instruction.5", "หลังผสมกันแล้ว ตั้งทิ้งไว้จนสารจับตัวเป็นก้อน");

        thLocale.setString("course.create_soap.fail.1", "ไม่มีสาร Sodium Hydroxide อยู่ในบีกเกอร์น้ำ");
        thLocale.setString("course.create_soap.fail.2", "สาร Sodium Hydroxide ไม่เพียงพอต่อการทำปฎิกิริยา");
        thLocale.setString("course.create_soap.fail.3", "ไม่มีน้ำมันมะพร้าวอยู่ในบีกเกอร์น้ำ");
        thLocale.setString("course.create_soap.fail.4", "น้ำมันมะพร้าวไม่เพียงพอต่อการทำปฎิกิริยา");

        LocalLanguage enLocale = courseBehaviour.globalObject.localization["en"];

        enLocale.setString("course.create_soap.instruction.1", "Fill Sodium Hydroxide into water");
        enLocale.setString("course.create_soap.instruction.2", "Stir and mixed the substance");
        enLocale.setString("course.create_soap.instruction.3", "Fill coconut oil");
        enLocale.setString("course.create_soap.instruction.4", "Stir and mixed the substance");
        enLocale.setString("course.create_soap.instruction.5", "After mixed, Rest it and wait utill substance is solid");

        enLocale.setString("course.create_soap.fail.1", "No Sodium Hydroxide in Breaker");
        enLocale.setString("course.create_soap.fail.2", "Not enough Sodium Hydroxide to make the reaction");
        enLocale.setString("course.create_soap.fail.3", "No coconut oil in Breaker");
        enLocale.setString("course.create_soap.fail.4", "Not enough coconut oil to make the reaction");
    }

    bool finishing = false;
    FinaleTrigger trigger = null;

    protected override void FinishCourse()
    {
        courseBehaviour.sceneController.HideAllCanvas();
        courseBehaviour.trackers[baseTrackerName].attachObject.gameObject.SetActive(false);

        // Disable highlights and Text
        foreach (TrackingImage ti in courseBehaviour.trackers.Values)
        {
            ti.enableTextMesh = false;
            ti.enableHighlightPlane = false;
        }

        // For Check :3
        //courseBehaviour.globalObject.gameResult.data.score = new System.Random().Next(0, 100);
        //courseBehaviour.globalObject.gameResult.data.time = new System.Random().Next(120, 600);

        courseBehaviour.globalObject.gameResult.data.score = 60;

        if (courseBehaviour.currentCourseTime < 300)
        {
            courseBehaviour.globalObject.gameResult.data.score += 10;
        }
        else if(courseBehaviour.currentCourseTime >= 300 && courseBehaviour.currentCourseTime < 500)
        {
            courseBehaviour.globalObject.gameResult.data.score += 6;
        }

        Volume soapLiquidVol = ((FillableEquipment)GetEquipmentByObjectName("Beaker_Water")).GetVolumeOfMaterial(SOAP_LIQUID);
        if (soapLiquidVol != null)
        {
            if (soapLiquidVol.volume >= 20 && soapLiquidVol.volume < 50)
            {
                courseBehaviour.globalObject.gameResult.data.score += 6;
            }
            else if (soapLiquidVol.volume >= 50 && soapLiquidVol.volume < 100)
            {
                courseBehaviour.globalObject.gameResult.data.score += 12;
            }
            else if (soapLiquidVol.volume >= 100 && soapLiquidVol.volume < 200)
            {
                courseBehaviour.globalObject.gameResult.data.score += 16;
            }
            else if (soapLiquidVol.volume >= 200 && soapLiquidVol.volume < 400)
            {
                courseBehaviour.globalObject.gameResult.data.score += 23;
            }
            else if (soapLiquidVol.volume >= 400)
            {
                courseBehaviour.globalObject.gameResult.data.score += 30;
            }
        }

        courseBehaviour.globalObject.gameResult.data.time = (int)(courseBehaviour.currentCourseTime);

        GameObject finalSoapObj = Instantiate(finalSoap);
        finalSoapObj.transform.SetParent(courseBehaviour.trackers[baseTrackerName].transform);
        finalSoapObj.transform.position = new Vector3(-0.906f, -3.406f, -0.501f);
        finalSoapObj.transform.localScale = new Vector3(0.13f, 0.13f, 0.13f);

        trigger = finalSoapObj.GetComponentInChildren<FinaleTrigger>();
        trigger.PlayAnimation();

        courseBehaviour.SubmitScore(courseBehaviour.globalObject.gameResult);
        finishing = true;
    }

    protected override void RestartCoruse()
    {
        FILL_SODIUM_HYDROXIDE_TO_WATER = false;
        MIX_SODIUM_HYDROXIDE_TO_WATER = false;
        FILL_MIXED_WATER_SODIUM_HYDROXIDE_TO_OIL = false;
        MIX_MIXED_WATER_SODIUM_HYDROXIDE_TO_OIL = false;

        beakerWater.enableFlow = true;
        plateSodiumHydroxide.enableFlow = true;
        bottleCoconutOil.enableFlow = true;

        updateInstruction = false;

        // Reset color
        FillableEquipment equipment = (FillableEquipment)GetEquipmentByObjectName("Beaker_Water");
        equipment.particleColor = new Color(160f / 255f, 208f / 255f, 249f / 255f, 1f);
    }

    private bool updateInstruction = false;

    protected override void UpdateCoruse()
    {
        LocalLanguage locale = courseBehaviour.globalObject.currentLocale;

        #region Checkpoint

        beakerWater.highlighting = false;
        plateSodiumHydroxide.highlighting = false;
        bottleCoconutOil.highlighting = false;

        if (!finishing)
        {
            FillableEquipment equipment = (FillableEquipment)GetEquipmentByObjectName("Beaker_Water");

            // Update color if contain Soap(liquid)
            if (equipment.ContainMaterial(SOAP_LIQUID))
            {
                Volume vol = equipment.GetVolumeOfMaterial(SOAP_LIQUID);

                if (vol.volume < 50f)
                {
                    equipment.particleColor = Color.Lerp(new Color(160f / 255f, 208f / 255f, 249f / 255f, 1f), new Color(208f / 255f, 208f / 255f, 208f / 255f, 1f), vol.volume / 50f);
                }
            }

            if (!FILL_SODIUM_HYDROXIDE_TO_WATER)
            {
                if(!updateInstruction)
                {
                    courseBehaviour.sceneController.ChangeInstructionMessage(locale.getString("course.create_soap.instruction.1"));
                    updateInstruction = true;
                }                

                if (!plateSodiumHydroxide.ContainMaterial(SODIUM_HYDROXIDE))
                {
                    // Check if contain
                    if (!equipment.ContainMaterial(SODIUM_HYDROXIDE))
                    {
                        courseBehaviour.FailCourse(locale.getString("course.create_soap.fail.1"));
                    }
                    else if(equipment.ContainMaterial(SODIUM_HYDROXIDE) && accumulateFillSodiumHydroxide >= 15f)
                    {
                        FILL_SODIUM_HYDROXIDE_TO_WATER = true;
                        updateInstruction = false;
                    }
                    else
                    {
                        courseBehaviour.FailCourse(locale.getString("course.create_soap.fail.2"));
                    }
                }

                plateSodiumHydroxide.highlighting = true;
            }
            else if (!MIX_SODIUM_HYDROXIDE_TO_WATER)
            {
                if (!updateInstruction)
                {
                    courseBehaviour.sceneController.ChangeInstructionMessage(locale.getString("course.create_soap.instruction.2"));
                    updateInstruction = true;
                }

                // Check contain & volume
                if (equipment.ContainMaterial(MIXED_WATER_SODIUM_HYDROXIDE) && !equipment.ContainMaterial(SODIUM_HYDROXIDE) && !equipment.ContainMaterial(WATER))
                {
                    MIX_SODIUM_HYDROXIDE_TO_WATER = true;
                    updateInstruction = false;
                }

                beakerWater.highlighting = true;
            }
            else if (!FILL_MIXED_WATER_SODIUM_HYDROXIDE_TO_OIL)
            {
                if (!updateInstruction)
                {
                    courseBehaviour.sceneController.ChangeInstructionMessage(locale.getString("course.create_soap.instruction.3"));
                    updateInstruction = true;
                }

                if (!bottleCoconutOil.ContainMaterial(COCONUT_OIL))
                {
                    // Check if contain
                    if (!equipment.ContainMaterial(COCONUT_OIL))
                    {
                        courseBehaviour.FailCourse(locale.getString("course.create_soap.fail.3"));
                    }
                    else if (equipment.ContainMaterial(COCONUT_OIL) && accumulateFillCoconutOil >= 100f)
                    {
                        FILL_MIXED_WATER_SODIUM_HYDROXIDE_TO_OIL = true;
                        updateInstruction = false;
                    }
                    else
                    {
                        courseBehaviour.FailCourse(locale.getString("course.create_soap.fail.4"));
                    }
                }

                bottleCoconutOil.highlighting = true;
            }
            else if (!MIX_MIXED_WATER_SODIUM_HYDROXIDE_TO_OIL)
            {
                if (!updateInstruction)
                {
                    courseBehaviour.sceneController.ChangeInstructionMessage(locale.getString("course.create_soap.instruction.4"));
                    updateInstruction = true;
                }

                // Check contain and volume
                if (equipment.ContainMaterial(SOAP_LIQUID) && !equipment.ContainMaterial(MIXED_WATER_SODIUM_HYDROXIDE) && !equipment.ContainMaterial(COCONUT_OIL))
                {
                    MIX_MIXED_WATER_SODIUM_HYDROXIDE_TO_OIL = true;
                    updateInstruction = false;
                    courseBehaviour.sceneController.ShowFinishButton();
                }

                beakerWater.highlighting = true;
            }
            else
            {
                if (!updateInstruction)
                {
                    courseBehaviour.sceneController.ChangeInstructionMessage(locale.getString("course.create_soap.instruction.5"));
                    updateInstruction = true;
                }

                beakerWater.highlighting = true;
            }
        }

        #endregion

        if (finishing)
        {
            if (trigger.soap1.GetCurrentAnimatorStateInfo(0).IsName("finishFallingSoap"))
            {
                SceneManager.LoadScene(2);
            }
        }
    }

    public void ForceFinishCourse() {
        FinishCourse();
    }

    protected override void FailCourse()
    {
        // disable flow
        beakerWater.enableFlow = false;
        plateSodiumHydroxide.enableFlow = false;
        bottleCoconutOil.enableFlow = false;

        // disable highlight
        beakerWater.highlighting = false;
        plateSodiumHydroxide.highlighting = false;
        bottleCoconutOil.highlighting = false;

        courseBehaviour.sceneController.ChangeInstructionMessage(courseBehaviour.globalObject.currentLocale.getString("course.fail"));
    }

    #region Handle event

    private void beforeUpdateEquipment(FillableEquipment equipment)
    {
        float stirAmplifier = equipment.stirAmplifier;

        #region If have Sodium Hydroxide and Water, increase temperature and mix it over time

        if (equipment.ContainMaterial(SODIUM_HYDROXIDE) && equipment.ContainMaterial(WATER))
        {
            // Mixed material
            if (!equipment.ContainMaterial(MIXED_WATER_SODIUM_HYDROXIDE))
            {
                equipment.Fill(new ChemistRecipe.Experiment.Material(MIXED_WATER_SODIUM_HYDROXIDE, ChemistRecipe.Experiment.Type.LIQUID), new Volume(0f, 25f, Volume.Metric.mL));
            }
            // Increase volume & temperature
            Volume vol = equipment.GetVolumeOfMaterial(MIXED_WATER_SODIUM_HYDROXIDE);
            vol.volume += 0.6875f * stirAmplifier * Time.deltaTime;
            vol.tempature += 0.0625f * stirAmplifier * Time.deltaTime;

            // Reduce Sodium Hydroxide volume
            Volume sh = equipment.GetVolumeOfMaterial(SODIUM_HYDROXIDE);
            sh.volume -= 0.0625f * stirAmplifier * Time.deltaTime;
            if (sh.volume <= 0)
            {
                equipment.removeMaterial(SODIUM_HYDROXIDE);
            }

            // Reduce Water volume
            Volume wa = equipment.GetVolumeOfMaterial(WATER);
            wa.volume -= 0.625f * stirAmplifier * Time.deltaTime;
            if (wa.volume <= 0)
            {
                equipment.removeMaterial(WATER);
            }
        }
        else if (equipment.ContainMaterial(MIXED_WATER_SODIUM_HYDROXIDE))
        {
            Volume vol = equipment.GetVolumeOfMaterial(MIXED_WATER_SODIUM_HYDROXIDE);

            // Mixed Sodium Hydroxide with MIXED_WATER_SODIUM_HYDROXIDE
            if (equipment.ContainMaterial(SODIUM_HYDROXIDE))
            {
                // Increase volume & temperature
                vol.volume += 0.625f * stirAmplifier * Time.deltaTime;
                vol.tempature += 0.0625f * stirAmplifier * Time.deltaTime;

                // Reduce Sodium Hydroxide volume
                Volume sh = equipment.GetVolumeOfMaterial(SODIUM_HYDROXIDE);
                sh.volume -= 0.625f * stirAmplifier * Time.deltaTime;
                if (sh.volume <= 0)
                {
                    equipment.removeMaterial(SODIUM_HYDROXIDE);
                }
            }

            // Mixed Water with MIXED_WATER_SODIUM_HYDROXIDE
            if (equipment.ContainMaterial(WATER))
            {
                // Increase volume & Reduce temperature
                vol.volume += 0.625f * stirAmplifier * Time.deltaTime;
                vol.tempature -= 0.005f * stirAmplifier * Time.deltaTime;

                // Reduce Water volume
                Volume wa = equipment.GetVolumeOfMaterial(WATER);
                wa.volume -= 0.625f * stirAmplifier * Time.deltaTime;
                if (wa.volume <= 0)
                {
                    equipment.removeMaterial(WATER);
                }
            }
        }

        #endregion
        
        #region If Have Water + Sodium Hydroxide and its Temp > 25c, cool it down over time

        if (equipment.ContainMaterial(MIXED_WATER_SODIUM_HYDROXIDE))
        {
            Volume vol = equipment.Materials[equipment.getMaterial(MIXED_WATER_SODIUM_HYDROXIDE)];
            vol.tempature -= 0.01f * Time.deltaTime;
        }

        #endregion

        #region If Have SOAP(Liquid) and its Temp > 25c, cool it down over time

        if (equipment.ContainMaterial(SOAP_LIQUID))
        {
            Volume vol = equipment.Materials[equipment.getMaterial(SOAP_LIQUID)];
            vol.tempature -= 0.03f * Time.deltaTime;
        }

        #endregion
    }

    private void stirEquipment(FillableEquipment equipment)
    {
        #region If Have Water + Sodium Hydroxide with Coconut Oil and Water + Sodium Hydroxide Temp >= 33c, mixed it!

        if (equipment.ContainMaterial(MIXED_WATER_SODIUM_HYDROXIDE) && equipment.ContainMaterial(COCONUT_OIL))
        {
            // Check Tempature (if below 33c, return)
            Volume MWSH_vol = equipment.GetVolumeOfMaterial(MIXED_WATER_SODIUM_HYDROXIDE);
            if (MWSH_vol.tempature < 35f)
            {
                return;
            }

            // Mixed material
            if (!equipment.ContainMaterial(SOAP_LIQUID))
            {
                equipment.Fill(new ChemistRecipe.Experiment.Material(SOAP_LIQUID, ChemistRecipe.Experiment.Type.LIQUID), new Volume(0f, MWSH_vol.tempature, Volume.Metric.mL));
            }

            equipment.Materials[equipment.getMaterial(SOAP_LIQUID)].volume += 27.5f;

            // Reduce material volume
            //
            // Water + Sodium Hydroxide
            Volume wsh = equipment.GetVolumeOfMaterial(MIXED_WATER_SODIUM_HYDROXIDE);
            wsh.volume -= 14.5f;
            if (wsh.volume <= 0)
            {
                equipment.removeMaterial(MIXED_WATER_SODIUM_HYDROXIDE);
            }

            // Coconut Oil
            Volume col = equipment.GetVolumeOfMaterial(COCONUT_OIL);
            col.volume -= 13f;
            if (col.volume <= 0)
            {
                equipment.removeMaterial(COCONUT_OIL);
            }
        }
        else if (equipment.ContainMaterial(SOAP_LIQUID))
        {
            Volume vol = equipment.GetVolumeOfMaterial(SOAP_LIQUID);

            // Mixed Water + Sodium Hydroxide with SOAP_LIQUID
            if (equipment.ContainMaterial(MIXED_WATER_SODIUM_HYDROXIDE))
            {
                float reduceVol = 14.5f;

                // Reduce Water + Sodium Hydroxide volume
                Volume wsh = equipment.GetVolumeOfMaterial(MIXED_WATER_SODIUM_HYDROXIDE);
                if (wsh.volume - reduceVol <= 0)
                {
                    reduceVol = reduceVol - wsh.volume;
                    equipment.removeMaterial(MIXED_WATER_SODIUM_HYDROXIDE);
                }
                else
                {
                    wsh.volume -= reduceVol;
                }

                // Increase volume & temperature
                vol.volume += reduceVol;
                vol.tempature += 0.625f;
            }

            // Mixed Coconut Oil with SOAP_LIQUID
            if (equipment.ContainMaterial(COCONUT_OIL))
            {
                float reduceVol = 13f;

                // Reduce Coconut Oil volume
                Volume col = equipment.GetVolumeOfMaterial(COCONUT_OIL);
                if (col.volume <= 0)
                {
                    reduceVol = reduceVol - col.volume;
                    equipment.removeMaterial(COCONUT_OIL);
                }
                else
                {
                    col.volume -= reduceVol;
                }

                // Increase volume & Reduce temperature
                vol.volume += reduceVol;
                vol.tempature -= 0.03f;
            }
        }

        #endregion
    }

    private void afterFillEquipment(FillableEquipment equipment, ChemistRecipe.Experiment.Material fillMaterial, Volume fillVol)
    {

        #region keep mat fill data

        if(equipment.name == beakerWater.name)
        {
            if (fillMaterial.name == SODIUM_HYDROXIDE)
            {
                accumulateFillSodiumHydroxide += fillVol.volume;
            }
            else if (fillMaterial.name == COCONUT_OIL)
            {
                accumulateFillCoconutOil += fillVol.volume;
            }
        }

        #endregion

        #region If have Water and Coconut Oil but not contain Water + Sodium Hydroxide, fail the course
        if (equipment.ContainMaterial(WATER) && !equipment.ContainMaterial(MIXED_WATER_SODIUM_HYDROXIDE) && fillMaterial.name == COCONUT_OIL)
        {
            courseBehaviour.FailCourse("น้ำกับน้ำมันมะพร้าวไม่ทำปฏิกิริยากัน");
        }

        #endregion
    }

    #endregion

}
