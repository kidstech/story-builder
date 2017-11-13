/// <summary>
/// A context pack contains words specific to a pre-defined context.
/// 
/// <author> antin006@morris.umn.edu </author>
/// </summary>
using System;
using UnityEngine;

namespace AssemblyCSharpfirstpass {
	
	public class ContextPack {

		// Context pack title
		public String title;

		// Words contained within context pack
		public String[] words;

		/// <summary>
		/// Initializes a new ContextPack.
		/// </summary>
		/// <param name="title">Title of the context pack</param>
		/// <param name="words">Words within context pack</param>
		public ContextPack(String title, String[] words){
			this.title = title;
			this.words = words;
		}

		/// <summary>
		/// Converts the context pack to JSON.
		/// </summary>
		/// <returns>Context pack as JSON with title and words</returns>
		public String ToJSON(){
			return JsonUtility.ToJson (this);
		}

		/// <summary>
		/// Returns a string that represents a context pack
		/// </summary>
		/// <returns>A string that represents a context pack</returns>
		public override String ToString(){
			String contextPack = "";

			contextPack += title;
			for (int i = 0; i < words.Length; ++i)
				contextPack += "\n\t" + words [i];

			return contextPack;
		}
	}
}

