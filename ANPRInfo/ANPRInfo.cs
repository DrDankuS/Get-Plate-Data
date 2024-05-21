using CitizenFX.Core;
using CitizenFX.Core.Native;
using CitizenFX.Core.UI;
using FivePD.API;
using FivePD.API.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANPRInfo
{
    internal class ANPRInfo : Plugin
    {
        private Vehicle lastStoppedVehicle;
        private VehicleData vehData;

        internal ANPRInfo() : base()
        {
            API.RegisterCommand("getANPR", new Action<int, List<object>, string>((source, args, rawCommand) =>
            {
                _ = getANPR();
            }), false);
            API.RegisterKeyMapping("getANPR", "Get Plate Data", "keyboard", "LSHIFT");
        }

        private async Task getANPR()
        {
            if (Utilities.IsPlayerPerformingTrafficStop())
            {
                lastStoppedVehicle = Utilities.GetVehicleFromTrafficStop();
                if (lastStoppedVehicle != null && lastStoppedVehicle.Exists())
                {
                    vehData = await lastStoppedVehicle.GetData();
                    ShowVehicleInfo(vehData);
                }
            }
        }

        private void ShowVehicleInfo(VehicleData vehData)
        {
            if (vehData != null)
            {
                string vehInsurance = vehData.Insurance == true ? "~r~Invalid~s~" : "Valid";
                string vehRegistration = vehData.Registration == true ? "~r~Invalid~s~" : "Valid";
                Debug.WriteLine(vehData.Flag);
                string vehFlags = vehData.Flag.Length > 1 ? $"~y~Flag:~r~ {vehData.Flag}~s~\n" : "";
                string vehicleInfo = $"~b~[Plate Check]~s~\n~y~Plate:~s~ {vehData.LicensePlate}\n~y~Model:~s~ {vehData.Name}\n~y~Color:~s~ {vehData.Color}\n~y~Owner:~s~ {vehData.OwnerFirstName} {vehData.OwnerLastName}\n{vehFlags}~y~Insurance~s~: {vehInsurance}\n~y~Registration~s~: {vehRegistration}";
                Screen.ShowNotification(vehicleInfo);
            }else
            {
                Debug.WriteLine("ERROR: VEHICLE DATA IS NULL.");
            }
        }

        
    }
}
