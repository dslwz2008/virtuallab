using UnityEngine;
using System.Collections;

public class Utils {
	public static bool IsValidIP(string ip)
    {
        if (System.Text.RegularExpressions.Regex.IsMatch(ip, "[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}"))
        {
            string[] ips = ip.Split('.');
            if (ips.Length == 4 || ips.Length == 6)
            {
                if (System.Int32.Parse(ips[0]) < 256 && System.Int32.Parse(ips[1]) < 256 & System.Int32.Parse(ips[2]) < 256 & System.Int32.Parse(ips[3]) < 256)
                    return true;
                else
                    return false;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

}
