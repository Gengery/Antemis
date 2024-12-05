using Antemis.Database;
using Antemis.Models;

namespace Antemis.ComplexModels
{
	public class WorkersListComplexModel
	{
		public List<WorkerMainInfoModel>? Workers {  get; set; }
		public WorkerSortComponents? Sort {  get; set; }
		public List<Work>? Works { get; set; }
	}
}
