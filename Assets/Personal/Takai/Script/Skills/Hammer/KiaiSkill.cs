
    using UnityEngine;
    using Cysharp.Threading.Tasks;
    using UnityEngine.Playables;

    public class KiaiSkill : SkillBase
    {
        public override string SkillName { get; protected set; }
        public override int Damage { get; protected set; }
        public override WeaponType Weapon { get; protected set; }
        public override SkillType Type { get; protected set; }
        private PlayableDirector _anim;

    public KiaiSkill()
    {
        SkillName = "気合い";
        Damage = 0;
        Weapon = (WeaponType)2;
        Type = (SkillType)0;
    }

        public async override UniTask UseSkill(PlayerStatus status)
        {
            Debug.Log("Use Skill");
            _anim = GetComponent<PlayableDirector>();
            SkillEffect(status);
            await UniTask.WaitUntil(() => _anim.state == PlayState.Paused);
            Debug.Log("Anim End");
        }

        protected override void SkillEffect(PlayerStatus status)
        {
            // スキルの効果処理を実装する
        }
    }