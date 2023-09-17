namespace ZE.Polytrucks {
	public enum Biome : byte { Default}
	public static class BiomeExtensions
	{
		public static Biome GetNext(this Biome biome) {
			var last = GetLast();
			if (biome == last) return 0; 
			else return ++biome; }
		public static Biome GetLast() => 0;
		public static bool IsLast(this Biome biome) => biome == GetLast();
		public static int GetTotalLevelsCount(this Biome biome) => 0;


		public static int GetAllAvailablesLevelCount()
		{
			int count = 0;
			for (Biome i = 0; i < GetLast(); i++)
			{
				count += i.GetTotalLevelsCount();
			}
			return count;
		}
	}
}
