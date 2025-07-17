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
        TutorialScene,
        GolemBossScene,
    }
    #endregion

    #region Sound
    // Resources 내의 BGM, SFX 폴더에 같은 이름으로 둬야함
    public enum BGM
    {
        None,
        TitleScene,
        InGameScene,
        EndingScene,
        GolemBossScene,
    }
    public enum SFX
    {
        None,
        StoneDoor,
        
        // 플레이어
        PlayerDash,
        PlayerHurt,
        PlayerDie,

        /* 아티팩트 */
        // 완드
        WandNormalAttack,
        WandSkillAttack,

        // 건틀렛
        GauntletNormalAttack,
        GauntletSkillAttack,

        // 손전등
        LightNormalAttack,
        LightSkillAttack,

        // 샷건
        // 일반 공격은 완드랑 같은 것을 사용
        ShotgunSkillAttack,

        // 소총
        AssultRifleNormalAttack,
        AssultRifleSkillAttack,

        // 피리
        FluteNormalAttack,
        FluteSkillAttack,

        GameOver,
        //CastDamageArea,
        CastExplosion,
        HitDefaultAttack,
        HitDamageArea,
        HitExplosion,
        HitGrab,
        Put,
        Pickup,
        Equip,
        sfx_slime_die, // 250611 KWS - test for enemy sound

        // 보스
        GolemRush,
        GolemRushWall,
        GolemSpiral,
        GolemLaser,
        GolemSpike,
        GolemDie,
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

    #region  파츠 정보
    public enum PartsType
    {
        After_AttackPower_1,
        AfterSkill_NormalAttackPower_1,
        BeforeSkill_Barrier_1,
        BeforeSkill_SkillAttackPower_1,
        Hit_Explosion_1,
        HitNormal_SkillAttackPower_1,
        HitSkill_AttackPower_1,
        HitSkill_MoveSpeed_1,
        HitSkill_SkillAttackPower_1,
        Kill_ManaGain_1,
        KillSkill_Explosion_1,
        Passive_AttackPower_1,
        Passive_ManaGain_1,
        Passive_MaxMana_1,
        Passive_MoveSpeed_1,
        Passive_NormalAttackCoolTime_1,
        Passive_NormalAttackPower_1,
        Passive_SkillAttackPower_1,
        Passive_SkillAttackPower_3,
    }
    #endregion

    #region Enemy
    public enum EnemyName
    {
        // 넘버링은 등장 스테이지:D2 + 번호:D2
        // 번호 넘버링은 근접계열 00 ~ 29, 원거리계열 30 ~ 59, 특수계열 60 ~ 89, 보스 90 ~ 99

        None = 0,

        Carapace_Slime = 0100,
        Worm_Slime = 0101,
        Jar_Larva = 0102,

        Spore_Slime = 0130,
        Scatter_Bat = 0131,
        Webber = 0132,
        
        Jar = 0160,
        Boom_Shroom = 0161,
        Blast_Shroom = 0162,

        Horn_Sludge = 0190, // 미니보스
        GolemBoss = 0191 // 최종보스
    }
    #endregion
}