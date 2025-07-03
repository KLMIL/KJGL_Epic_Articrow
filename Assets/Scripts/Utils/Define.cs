public class Define
{
    #region Scene
    // 씬 타입(이름)
    public enum SceneType
    {
        None,
        TitleScene,
        InGameScene,
        EndingScene,
        TutorialScene
    }
    #endregion

    #region Sound
    // Resources 내의 BGM, SFX 폴더에 같은 이름으로 둬야함
    public enum BGM
    {
        None,
        TitleScene,
        InGameScene,
        EndingScene
    }
    public enum SFX
    {
        None,
        DoorOpen,
        DoorClose,
        DefaultAttack,
        Attack1,
        Attack2,
        Slash,
        CastDamageArea,
        CastExplosion,
        HitDefaultAttack,
        HitDamageArea,
        HitExplosion,
        HitGrab,
        Put,
        Pickup,
        sfx_slime_die // 250611 KWS - test for enemy sound
    }
    #endregion

    public enum KeyAction
    {
        Move,
        Interact,
        Inventroy,
        LeftHand,
        RightHand,
        Dash,
    }

    #region 맵
    // 방 종류
    public enum RoomType
    {
        None,
        StartRoom,      // 시작
        BossRoom,       // 보스
        MagicRoom,      // 마법
        ArtifactRoom,   // 아티팩트
        HealRoom,       // 회복
        TutorialRoom,   // 튜토리얼
    }

    // 문 방향
    public enum DoorPosition
    {
        None = 0,
        Up = 1,
        Left = 2,
        Right = 3,
        Down = 4,
    }
    #endregion

    #region 풀링
    // 풀링 타입
    public enum PoolType
    {
        None,
        EnemyPool,
        ProjectilePool,
        ItemPool,
        EffectPool,
        TextPool,
        SkillPool
    }

    // 풀링할 오브젝트에 부여할 ID
    public enum PoolID
    {
        None,
        DamageText, // UI
        CastExplosion,
        HitExplosion,
        Description
    }
    #endregion

    #region [스킬 종류]
    public enum SkillType 
    { 
        Cast, 
        Hit, 
        Passive 
    }
    #endregion
}