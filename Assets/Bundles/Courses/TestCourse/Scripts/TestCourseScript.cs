using ChemistRecipe.Experiment;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.SceneManagement;

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

    protected override void SetupCoruse()
    {
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
        }

        courseBehaviour.sceneController.ShowFinishButton();
    }

    bool finishing = false;
    FinaleTrigger trigger = null;

    protected override void FinishCourse()
    {
        courseBehaviour.sceneController.HideAllCanvas();
        courseBehaviour.trackers[baseTrackerName].attachObject.gameObject.SetActive(false);

        courseBehaviour.globalObject.gameResult.data.score = new System.Random().Next(0, 100);
        //courseBehaviour.globalObject.gameResult.data.time = (int)(courseBehaviour.currentCourseTime);
        courseBehaviour.globalObject.gameResult.data.time = new System.Random().Next(120, 600);

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
    }

    protected override void UpdateCoruse()
    {

        #region Checkpoint

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
                courseBehaviour.sceneController.ChangeInstructionMessage("ทำการเทโซเดียมไฮดอรกไซด์ผสมกับน้ำ");

                // Check contain
                if (equipment.ContainMaterial(SODIUM_HYDROXIDE))
                {
                    FILL_SODIUM_HYDROXIDE_TO_WATER = true;
                }
            }
            else if (!MIX_SODIUM_HYDROXIDE_TO_WATER)
            {
                courseBehaviour.sceneController.ChangeInstructionMessage("ทำการคนให้สารผสมกัน");

                // Check contain & volume
                if (equipment.ContainMaterial(MIXED_WATER_SODIUM_HYDROXIDE))
                {
                    Volume vol = equipment.GetVolumeOfMaterial(MIXED_WATER_SODIUM_HYDROXIDE);

                    if (vol.volume > 20f)
                    {
                        MIX_SODIUM_HYDROXIDE_TO_WATER = true;
                    }
                }
            }
            else if (!FILL_MIXED_WATER_SODIUM_HYDROXIDE_TO_OIL)
            {
                courseBehaviour.sceneController.ChangeInstructionMessage("ทำการเทน้ำมันมะพร้าว");

                // Check contain and volume
                if (equipment.ContainMaterial(COCONUT_OIL))
                {
                    Volume vol = equipment.GetVolumeOfMaterial(COCONUT_OIL);
                    
                    FILL_MIXED_WATER_SODIUM_HYDROXIDE_TO_OIL = true;
                }
            }
            else if (!MIX_MIXED_WATER_SODIUM_HYDROXIDE_TO_OIL)
            {
                courseBehaviour.sceneController.ChangeInstructionMessage("ทำการคนให้สารผสมกัน");

                // Check contain and volume
                if (equipment.ContainMaterial(SOAP_LIQUID))
                {
                    Volume vol = equipment.GetVolumeOfMaterial(SOAP_LIQUID);

                    if (vol.volume > 20f)
                    {
                        MIX_MIXED_WATER_SODIUM_HYDROXIDE_TO_OIL = true;
                        courseBehaviour.sceneController.ShowFinishButton();
                    }
                }
            }
            else
            {
                courseBehaviour.sceneController.ChangeInstructionMessage("หลังผสมกันแล้ว ตั้งทิ้งไว้จนสารจับตัวเป็นก้อน");
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
            if (MWSH_vol.tempature < 33f)
            {
                return;
            }

            // Mixed material
            if (!equipment.ContainMaterial(SOAP_LIQUID))
            {
                equipment.Fill(new ChemistRecipe.Experiment.Material(SOAP_LIQUID, ChemistRecipe.Experiment.Type.LIQUID), new Volume(0f, MWSH_vol.tempature, Volume.Metric.mL));
            }

            equipment.Materials[equipment.getMaterial(SOAP_LIQUID)].volume += 2.75f;

            // Reduce material volume
            //
            // Water + Sodium Hydroxide
            Volume wsh = equipment.GetVolumeOfMaterial(MIXED_WATER_SODIUM_HYDROXIDE);
            wsh.volume -= 1.45f;
            if (wsh.volume <= 0)
            {
                equipment.removeMaterial(MIXED_WATER_SODIUM_HYDROXIDE);
            }

            // Coconut Oil
            Volume col = equipment.GetVolumeOfMaterial(COCONUT_OIL);
            col.volume -= 1.3f;
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
                float reduceVol = 1.45f;

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
                vol.tempature += 0.0625f;
            }

            // Mixed Coconut Oil with SOAP_LIQUID
            if (equipment.ContainMaterial(COCONUT_OIL))
            {
                float reduceVol = 1.3f;

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
                vol.tempature -= 0.003f;
            }
        }

        #endregion
    }

    #endregion

}
