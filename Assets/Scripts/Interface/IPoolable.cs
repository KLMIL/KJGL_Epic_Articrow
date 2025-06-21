/// <summary>
/// 풀링할 객체가 상속받을 인터페이스
/// </summary>
public interface IPoolable
{
    // 사용하기 전에 준비할 것
    public void ReadyToUse();
}