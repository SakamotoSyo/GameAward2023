
    using UnityEngine;
    using Cysharp.Threading.Tasks;
    using UnityEngine.Playables;

    public class NidangiriSkill : SkillBase
    {
        public override string SkillName { get; protected set; }
        public override int Damage { get; protected set; }
        public override WeaponType Weapon { get; protected set; }
        public override SkillType Type { get; protected set; }
        private PlayableDirector _anim;

    public NidangiriSkill()
    {
        SkillName = "2段斬り";
        Damage = 80;
        Weapon = (WeaponType)0;
        Type = (SkillType)0;
    }

        public async override UniTask UseSkill(PlayerStatus status)
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