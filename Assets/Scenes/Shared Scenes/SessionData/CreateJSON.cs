using System.Collections;
using UnityEngine;
using DatabaseEntry;
using System.IO;

public class CreateJSON : MonoBehaviour
{
    public void CreateFile()
    {
        string folderName = @"d:\Testing";
        string pathString = System.IO.Path.Combine(folderName, "SubFolder");
        System.IO.Directory.CreateDirectory(pathString);
        string fileName = "testfile";
        pathString = System.IO.Path.Combine(pathString, fileName);

        Debug.Log("Path to file: " + pathString);

        if (!System.IO.File.Exists(pathString))
        {
            using (System.IO.FileStream fs = System.IO.File.Create(pathString))
            {
                for (byte i = 0; i < 100; i++)
                {
                    fs.WriteByte(i);
                }
            }
        }
        else
        {
            Debug.Log("File " + fileName + " already exists.");
            return;
        }
        try
        {
            byte[] readBuffer = System.IO.File.ReadAllBytes(pathString);
            foreach (byte b in readBuffer)
            {
                Debug.Log(b + " ");
            }
        }
        catch(System.IO.FileLoadException e)
        {
            Debug.Log(e.Message);
        }
    }
}

