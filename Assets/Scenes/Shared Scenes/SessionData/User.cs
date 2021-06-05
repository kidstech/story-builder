using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Collections;
namespace ServerTypes
{
    public class User
    {
        public string _id;
        public string authId;
        public string name;
        public string icon;
        // list of type learner
        public ArrayList learners;
        // list of strings
        public ArrayList contextPacks;
    }
}