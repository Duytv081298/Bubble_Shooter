using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Spine;
using Spine.Unity;

public class EffectsController : MonoBehaviour
{

    public ParticleSystem[] effect_Bubbles;
    [SerializeField]
    private SkeletonAnimation dino_animation;
    Spine.AnimationState dino_animation_state;
    [SerializeField]
    private SkeletonAnimation owl_animation;
    Spine.AnimationState owl_animation_state;
    private bool wait_dino_animation = false;
    private bool wait_owl_animation = false;


    void Start()
    {
        dino_animation_state = dino_animation.AnimationState;
        owl_animation_state = owl_animation.AnimationState;
    }

    // Update is called once per frame
    void Update()
    {
    }
    public void PlayEffect(int effectID, Vector2 position)
    {
        ParticleSystem effect = Instantiate(effect_Bubbles[effectID], position, Quaternion.identity, transform);
    }
    public void StartAnimationDino_Owl(){
        StartAnimationDino();
        StartAnimationOwl();
    }
    public void StartAnimationDino()
    {
        TrackEntry entry = dino_animation_state.SetAnimation(0, "Start", false);
        entry.Complete += TrackComplete;
        void TrackComplete(TrackEntry trackEntry)
        {
            wait_dino_animation = true;
            RandomIdleDino();
        }
    }
    public void RandomIdleDino()
    {
        if (wait_dino_animation)
        {
            var i = Random.Range(0, 3);
            string nameIdle = "";
            switch (i)
            {
                case 0:
                    nameIdle = "Idle";
                    break;
                case 1:
                    nameIdle = "Idle2";
                    break;
                case 2:
                    nameIdle = "Idle3";
                    break;
            }
            PlayIdleDino(nameIdle);
        }
    }
    public void PlayIdleDino(string name)
    {
        TrackEntry entry = dino_animation_state.SetAnimation(0, name, false);
        entry.Complete += TrackComplete;
        void TrackComplete(TrackEntry trackEntry)
        {
            RandomIdleDino();
        }
    }
    public void StartAnimationOwl()
    {

        TrackEntry entry = owl_animation_state.SetAnimation(0, "Celeb", false);
        entry.Complete += TrackComplete;
        void TrackComplete(TrackEntry trackEntry)
        {
            wait_owl_animation = true;
            RandomIdleOwl();
        }
    }
    public void RandomIdleOwl()
    {
        if (wait_owl_animation)
        {
            var i = Random.Range(0, 2);
            string nameIdle = "";
            switch (i)
            {
                case 0:
                    nameIdle = "Idle";
                    break;
                case 1:
                    nameIdle = "Idle2";
                    break;
            }
            PlayIdleOwl(nameIdle);
        }
    }
      public void PlayIdleOwl(string name)
    {
        TrackEntry entry = owl_animation_state.SetAnimation(0, name, false);
        entry.Complete += TrackComplete;
        void TrackComplete(TrackEntry trackEntry)
        {
            RandomIdleOwl();
        }
    }

    public void SetAnimation(string name)
    {
        // dino_animation.AnimationName = name;
        TrackEntry entry = dino_animation_state.SetAnimation(0, name, false);
        // entry.Complete += TrackComplete;
    }
    void TrackComplete(TrackEntry trackEntry)
    {
        Debug.Log("Complete");
        Debug.Log(trackEntry.Animation.Duration);

    }

}
