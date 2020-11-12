using SaveParser.Utils;
using SaveParser.Utils.BitStreams;

namespace SaveParser.Parser.StateFile {
	
	public class EntityPatchFile : EmbeddedStateFile {
		
		public EntityPatchFile(SourceSave saveRef, CharArray name) : base(saveRef, name) {}


		protected override void Parse(ref BitStreamReader bsr) {
			base.Parse(ref bsr);
			/*
			 * for ( i = 0; i < size; i++ )
		{
			g_pSaveRestoreFileSystem->Read( &entityId, sizeof(int), pFile );
			pSaveData->GetEntityInfo(entityId)->flags = FENTTABLE_REMOVED;
		}
			 */
		}


		public override void AppendToWriter(IIndentedWriter iw) {
			iw.Append(Name);
			iw.FutureIndent++;
			iw.Append($"\nmws: {Id}");
			iw.FutureIndent--;
		}
	}
}