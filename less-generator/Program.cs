using System;
using System.Text;

namespace lessgen
{
	class MainClass
	{
		const int DefaultColumns = 12;
		const string DefaultPath = "";

		public static void Main (string[] args)
		{
			int columnsToGenerate = DefaultColumns;
			string path = DefaultPath;
			if (args.Length >= 1) {
				columnsToGenerate = Int32.Parse (args [0]);
			}
			if (args.Length >= 2) {
				path = args [1];
			}

			for (int index = 2; index <= columnsToGenerate; index++) {
				Console.WriteLine ("Generating column: {0}", index);
				GenerateFile (index, path);
			}
		}

		private static void GenerateFile(int columns, string path) {
			var filename = columns + "cols.less";

			var content = BuildLess (columns);
			Console.WriteLine ("LESS file generated");

			var fs = new System.IO.StreamWriter (path + filename);
			fs.Write (content);
			fs.Close ();

			Console.WriteLine ("Created LESS file {0}", filename);
		}

		private static string BuildLess(int columns) {
			StringBuilder sb = new StringBuilder();

			sb.Append("@import \"common.less\";\n");
			sb.AppendLine ();
			sb.AppendFormat("@columns: {0};\n", columns);
			sb.AppendLine ();

			// Generate spans
			for (int index = 0; index < columns; index++) {
				sb.AppendFormat (".span_{0}_of_{1} {{ .calculateSpan(@columns, {0}); }}\n", index + 1, columns);
			}

			sb.AppendLine ();

			// Generate offsets
			for (int index = 0; index < columns; index++) {
				sb.AppendFormat (".col.offset_{0}_of_{1} {{ .calculateOffset(@columns, {0}); }}\n", index + 1, columns);
			}

			sb.Append ("\n\n/*  GO FULL WIDTH  */\n\n@media only screen and (max-width: @fullWidthUnder) {\n");

			// Mobile spans
			for (int index = columns; index > 1; index--) {
				sb.AppendFormat ("\t.span_{0}_of_{1},\n", index, columns);
			}
			sb.AppendFormat ("\t.span_1_of_{0} {{\n", columns);
			sb.Append ("\t\twidth: 100%;\n");
			sb.Append ("\t}\n");

			sb.AppendLine ();

			// Mobile offsets
			for (int index = columns; index > 1; index--) {
				sb.AppendFormat ("\t.col.offset_{0}_of_{1},\n", index, columns);
			}
			sb.AppendFormat ("\t.col.offset_1_of_{0} {{\n", columns);
			sb.Append ("\t\tmargin: 1% 0 1% 0;\n");
			sb.Append ("\t}\n");

			sb.Append ("}\n");

			return sb.ToString ();
		}
	}
}
