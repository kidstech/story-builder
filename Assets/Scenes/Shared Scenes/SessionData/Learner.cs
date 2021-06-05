using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Collections;
namespace ServerTypes
{
    public class Learner
    {
        public string _id;
        public string name;
        public string icon;
        // list of strings
        public ArrayList learnerPacks;
    }
}