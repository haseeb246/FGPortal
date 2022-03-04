using System.Collections.Generic;

namespace FGPortal.Models
{
	public class PreferencesModel
	{
		public string Username { get; set; }

		public List<UserPreference> UserPreferences { get; set; }

		public List<UserRestriction> UserRestrictions { get; set; }

		public PreferencesModel()
		{
			UserPreferences = new List<UserPreference>();
			UserRestrictions = new List<UserRestriction>();
		}
	}
}
