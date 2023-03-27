using UnityEngine;
using System.Collections;
namespace ServerTypes
{
    public class User
    {
        public string _id;
        public string authId;
        public string name;
        public string icon;
        // array of learners
        public Learner[] learners;
        // list of strings
        public ArrayList contextPacks;
    }
}