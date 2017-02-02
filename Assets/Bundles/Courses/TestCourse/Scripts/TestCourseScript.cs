using ChemistRecipe.Experiment;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class TestCourseScript : CourseScript
{
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
                beforeUpdate(equipment);
            };
            equipment.OnStir = () =>
            {
                stir(equipment);
            };
        }
    }

    protected override void FinishCourse()
    {
        
    }

    protected override void RestartCoruse()
    {
        
    }

    protected override void UpdateCoruse()
    {
        
    }

    #region Handle event

    private void beforeUpdate(FillableEquipment equipment)
    {
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
            vol.volume += 0.6875f * Time.deltaTime;
            vol.tempature += 0.0625f * Time.deltaTime;

            // Reduce Sodium Hydroxide volume
            Volume sh = equipment.GetVolumeOfMaterial(SODIUM_HYDROXIDE);
            sh.volume -= 0.0625f * Time.deltaTime;
            if (sh.volume <= 0)
            {
                equipment.removeMaterial(SODIUM_HYDROXIDE);
            }

            // Reduce Water volume
            Volume wa = equipment.GetVolumeOfMaterial(WATER);
            wa.volume -= 0.625f * Time.deltaTime;
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
                vol.volume += 0.625f * Time.deltaTime;
                vol.tempature += 0.0625f * Time.deltaTime;

                // Reduce Sodium Hydroxide volume
                Volume sh = equipment.GetVolumeOfMaterial(SODIUM_HYDROXIDE);
                sh.volume -= 0.625f * Time.deltaTime;
                if (sh.volume <= 0)
                {
                    equipment.removeMaterial(SODIUM_HYDROXIDE);
                }
            }

            // Mixed Water with MIXED_WATER_SODIUM_HYDROXIDE
            if (equipment.ContainMaterial(WATER))
            {
                // Increase volume & Reduce temperature
                vol.volume += 0.625f * Time.deltaTime;
                vol.tempature -= 0.005f * Time.deltaTime;

                // Reduce Water volume
                Volume wa = equipment.GetVolumeOfMaterial(WATER);
                wa.volume -= 0.625f * Time.deltaTime;
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
    }

    private void stir(FillableEquipment equipment)
    {
        #region If equipment contain Sodium Hydroxide and Water, mixed it!

        if (equipment.ContainMaterial(SODIUM_HYDROXIDE) && equipment.ContainMaterial(WATER))
        {
            // Mixed material
            if (!equipment.ContainMaterial(MIXED_WATER_SODIUM_HYDROXIDE))
            {
                equipment.Fill(new ChemistRecipe.Experiment.Material(MIXED_WATER_SODIUM_HYDROXIDE, ChemistRecipe.Experiment.Type.LIQUID), new Volume(11f, 26f, Volume.Metric.mL));
            }
            else
            {
                Volume vol = equipment.Materials[equipment.getMaterial(MIXED_WATER_SODIUM_HYDROXIDE)];
                vol.volume += 11f;
                vol.tempature += 1f;
            }

            // Reduce material volume
            //
            // Sodium Hydroxide
            Volume sh = equipment.GetVolumeOfMaterial(SODIUM_HYDROXIDE);
            sh.volume -= 1f;
            if (sh.volume <= 0)
            {
                equipment.removeMaterial(SODIUM_HYDROXIDE);
            }

            // Water
            Volume wa = equipment.GetVolumeOfMaterial(WATER);
            wa.volume -= 10f;
            if (wa.volume <= 0)
            {
                equipment.removeMaterial(WATER);
            }
        }

        #endregion

        // Mix remain water or sodium hydroxide with mixed_water_sodium_hydroxide

        #region If Have Water + Sodium Hydroxide with Coconut Oil

        if (equipment.ContainMaterial(MIXED_WATER_SODIUM_HYDROXIDE) && equipment.ContainMaterial(COCONUT_OIL))
        {
            // Mixed material
            if (!equipment.ContainMaterial(SOAP_LIQUID))
            {
                equipment.Fill(new ChemistRecipe.Experiment.Material(SOAP_LIQUID, ChemistRecipe.Experiment.Type.LIQUID), new Volume(29.5f, Volume.Metric.mL));
            }
            else
            {
                equipment.Materials[equipment.getMaterial(SOAP_LIQUID)].volume += 11f;
            }

            // Reduce material volume
            //
            // Sodium Hydroxide
            Volume sh = equipment.GetVolumeOfMaterial(MIXED_WATER_SODIUM_HYDROXIDE);
            sh.volume -= 5f;
            if (sh.volume <= 0)
            {
                equipment.removeMaterial(MIXED_WATER_SODIUM_HYDROXIDE);
            }

            // Water
            Volume wa = equipment.GetVolumeOfMaterial(COCONUT_OIL);
            wa.volume -= 13f;
            if (wa.volume <= 0)
            {
                equipment.removeMaterial(COCONUT_OIL);
            }
        }

        #endregion
    }    

    #endregion

}
