/// <summary>
/// The ContextPackFactory class handles local file management of ContextPacks. Tasks include building a master context pack JSON file from local text files and
/// loading context packs from the master JSON file.
/// 
/// <author>antin006@morris.umn.edu</author>
/// </summary>
using System;
using UnityEngine;
using System.Collections.Generic;

namespace AssemblyCSharpfirstpass {

	public class ContextPackFactory {

		/// <summary> File name for the master context pack JSON file. </summary>
		public static readonly String CONTEXT_PACK_FILE_NAME = "context_packs";

		/// <summary> File path for the master context pack JSON file. </summary>
		public static readonly String CONTEXT_PACK_FILE_PATH = Application.dataPath + "/Resources/context_packs";

		/// <summary> Delimeter between JSON files in the master context pack JSON file. </summary>
		public static readonly String JSON_DELIMETER = "\n";

		/// <summary> Local context pack text files stored within the Resources directory. </summary>
		public static readonly String[] LOCAL_CONTEXT_PACK_NAMES = { "Batman", "Minecraft", "MyLittlePony", "SpongebobSquarepants" };



		/// <summary>
		/// A ContextPackFactory should not be instantiated as its purpose is to manage context packs as a singleton.
		/// </summary>
		private ContextPackFactory() {}

		/// <summary>
		/// Loads context packs from the master context pack JSON file.
		/// The master context pack file is identified by the file represented by the CONTEXT_PACK_FILE_PATH constant.
		/// </summary>
		/// <returns>All context packs stored within the master context pack JSON file.</returns>
		public static ContextPack[] loadContextPacks(){

			// Load the master context pack JSON file
			TextAsset allcontextPacksJSON = (TextAsset) Resources.Load (CONTEXT_PACK_FILE_NAME);

			// Split each JSON around the JSON delimeter
			String[] contextPacksJSON = allcontextPacksJSON.text.Split (new String[]{JSON_DELIMETER}, StringSplitOptions.None);

			// Store each context pack read from JSON
			ContextPack[] contextPacks = new ContextPack[contextPacksJSON.Length];

			// Convert each JSON context pack to a ContextPack object
			for (int i = 0; i < contextPacks.Length; i++)
				contextPacks [i] = JsonUtility.FromJson<ContextPack> (contextPacksJSON [i]);

			return contextPacks;
		}

		/// <summary>
		/// Builds a master context pack JSON file from the file names stored within LOCAL_CONTEXT_PACK_NAMES.
		/// This is intended for use on Windows development environments to provide a streamlined way of generating a master JSON file.
		/// This is NOT used during the runtime of StoryBuilder on any platform.
		/// </summary>
		public static void buildContextPacks(){

			// Store read context packs
			ContextPack[] packs = new ContextPack[LOCAL_CONTEXT_PACK_NAMES.Length];

			// Parse each context pack .txt file to create each ContextPack object
			for (int i = 0; i < LOCAL_CONTEXT_PACK_NAMES.Length; ++i) {

				// Read context pack text file
				TextAsset contextPack = (TextAsset) Resources.Load (LOCAL_CONTEXT_PACK_NAMES[i]);

				// Split around word delimeters
				String[] words = contextPack.text.Split (new String[]{"\r\n", "\n"}, StringSplitOptions.None);

				// Construct context pack
				packs [i] = new ContextPack (LOCAL_CONTEXT_PACK_NAMES [i], words);
			}

			// Store all context packs as concatenated JSON
			String allContextPacksJSON = "";

			// Concatenate each JSON context pack to make master JSON file
			for (int i = 0; i < packs.Length; ++i)
				// Unless on last context pack, append a delimeter to each context pack
				allContextPacksJSON += packs [i].ToJSON () + (i == packs.Length - 1 ? "" : JSON_DELIMETER);

			// Write the master JSON to resources
			System.IO.File.WriteAllText (CONTEXT_PACK_FILE_PATH + ".json", allContextPacksJSON);

		}

	}
}
