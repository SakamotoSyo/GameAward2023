
    using UnityEngine;
    using Cysharp.Threading.Tasks;
    using UnityEngine.Playables;

    public class IatsuSKill : SkillBase
    {
        public override string SkillName { get; protected set; }
        public override int Damage { get; protected set; }
        public override WeaponType Weapon { get; protected set; }
        public override SkillType Type { get; protected set; }
        public override string FlavorText { get; protected set; }
        
        private PlayableDirector _anim;
        private PlayerStatus _status;
        private const int PowerDown = 0;

    public IatsuSKill()
    {
        SkillName = "威圧";
        Damage = 0;
        Weapon = (WeaponType)0;
        Type = (SkillType)0;
    }

        public async override UniTask UseSkill(PlayerStatus player, EnemyStatus enemy, WeaponStatus weapon)
        {
            Debug.Log("Use Skill");
            _status = player;
            _anim = GetComponent<PlayableDirector>();
            SkillEffect();
            await UniTask.WaitUntil(() => _anim.state == PlayState.Paused);
            Debug.Log("Anim End");
        }

        protected override void SkillEffect()
        {
            // スキルの効果処理を実装する
            
        }

        public override void TurnEnd()
        {
            
        }

        public override void BattleFinish()
        {
            
        }
    }