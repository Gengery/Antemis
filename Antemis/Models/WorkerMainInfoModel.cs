using Antemis.Database;

namespace Antemis.Models
{
	public class WorkerMainInfoModel
	{
		public WorkerMainInfoModel() { }

		public WorkerMainInfoModel(Person person, string job, string img)
		{
			this.INN = person.Inn;
			this.Surname = person.Surname;
			this.Name = person.Name;
			this.Patronimic = person.Patronimic;
			this.JobName = job;
			this.Img = img;
		}

		public string? INN { get; set; }
		public string? Surname { get; set; }
		public string? Name { get; set; }
		public string? Patronimic { get; set; }
		public string? JobName { get; set; }
		public string? Img { get; set; }
	}
}
