
    using UnityEngine;
    using Cysharp.Threading.Tasks;
    using UnityEngine.Playables;

    public class KyoukaSkill : SkillBase
    {
        public override string SkillName { get; protected set; }
        public override int Damage { get; protected set; }
        public override WeaponType Weapon { get; protected set; }
        public override SkillType Type { get; protected set; }
        private PlayableDirector _anim;

    public KyoukaSkill()
    {
        SkillName = "狂化";
        Damage = 0;
        Weapon = (WeaponType)0;
        Type = (SkillType)0;
    }

        public async override UniTask UseSkill()
        {
            Debug.Log("Use Skill");
            _anim = GetComponent<PlayableDirector>();
            await UniTask.WaitUntil(() => _anim.state == PlayState.Paused);
            Debug.Log("Anim End");
        }

        protected override void SkillEffect()
        {
            // スキルの効果処理を実装する
        }
    }