


public interface ISnailFishNumber
{
	ISnailFishNumber Reduce();
	(bool exploded, ISnailFishNumber number, int left, int right) Explode(int depth);
	(bool splitted, ISnailFishNumber number) Split();
	ISnailFishNumber Add(ISnailFishNumber another);
	ISnailFishNumber WithLeftIncreased(int value);
	ISnailFishNumber WithRightIncresed(int value);
	int GetMagnitude();
}
