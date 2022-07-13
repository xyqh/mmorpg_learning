using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Entities;
using SkillBridge.Message;
using Services;

public class PlayerInputController : MonoBehaviour {

    public Rigidbody rb;
    SkillBridge.Message.CharacterState state;

    private Character _character;

    public Character character {
        set
        {
            UnityLogger.stacktraceLog("PlayerInputController character null");
            _character = value;
        }
        get
        {
            return _character;
        }
    }

    public float rotateSpeed = 2.0f;

    public float turnAngle = 10;

    public int speed;

    public EntityController entityController;

    public bool onAir = false;

    public bool isActive = true;

    // Use this for initialization
    void Start () {
        state = SkillBridge.Message.CharacterState.Idle;
        if(this.character == null)
        {
            DataManager.Instance.Load();
            NCharacterInfo cinfo = new NCharacterInfo();
            cinfo.Id = 1;
            cinfo.Name = "Test";
            cinfo.Tid = 1;
            cinfo.Entity = new NEntity();
            cinfo.Entity.Position = new NVector3();
            cinfo.Entity.Direction = new NVector3();
            cinfo.Entity.Direction.X = 0;
            cinfo.Entity.Direction.Y = 100;
            cinfo.Entity.Direction.Z = 0;
            this.character = new Character(cinfo);

            if (entityController != null) entityController.entity = this.character;
        }
    }


    void FixedUpdate()
    {
        if (character == null || !isActive)
            return;

        // 获取sw 上下的输入来控制前后的移动
        float v = Input.GetAxis("Vertical");
        // 大于一定值才触发移动
        if (v > 0.01)
        {
            // 状态不是move的时候的处理，转成move状态。why 释放技能也能移动？
            if (state != SkillBridge.Message.CharacterState.Move)
            {
                state = SkillBridge.Message.CharacterState.Move;
                this.character.MoveForward();
                this.SendEntityEvent(EntityEvent.MoveFwd);
            }
            // 赋予刚体速度，当前速度 + 固定速度。匀加速运动，永无上限？
            this.rb.velocity = this.rb.velocity.y * Vector3.up + GameObjectTool.LogicToWorld(character.direction) * (this.character.speed + 9.81f) / 100f;
        }
        else if (v < -0.01)
        {
            if (state != SkillBridge.Message.CharacterState.Move)
            {
                state = SkillBridge.Message.CharacterState.Move;
                this.character.MoveBack();
                this.SendEntityEvent(EntityEvent.MoveBack);
            }
            this.rb.velocity = this.rb.velocity.y * Vector3.up + GameObjectTool.LogicToWorld(character.direction) * (this.character.speed + 9.81f) / 100f;
        }
        else
        {
            // 判断不是移动的时候，状态不是idle就转存idle。why 释放技能不按就变成站？
            if (state != SkillBridge.Message.CharacterState.Idle)
            {
                state = SkillBridge.Message.CharacterState.Idle;
                this.rb.velocity = Vector3.zero;
                this.character.Stop();
                this.SendEntityEvent(EntityEvent.Idle);
            }
        }

        // 输入跳就发送跳
        if (Input.GetButtonDown("Jump"))
        {
            this.SendEntityEvent(EntityEvent.Jump);
        }

        // 获取ad 左右，调整朝向，但不移动
        float h = Input.GetAxis("Horizontal");
        if (h<-0.1 || h>0.1)
        {
            // 设置脚本类的旋转角度
            this.transform.Rotate(0, h * rotateSpeed, 0);
            // 获取角色的方向
            Vector3 dir = GameObjectTool.LogicToWorld(character.direction);
            // 把从dir到rotate的欧拉角旋转转成四元数
            Quaternion rot = new Quaternion();
            rot.SetFromToRotation(dir, this.transform.forward);
            
            // 转一圈过后大于设定的角度10°的话，重设角色的方向，刚体的朝向
            if(rot.eulerAngles.y > this.turnAngle && rot.eulerAngles.y < (360 - this.turnAngle))
            {
                character.SetDirection(GameObjectTool.WorldToLogic(this.transform.forward));
                rb.transform.forward = this.transform.forward;
                this.SendEntityEvent(EntityEvent.None);
            }

        }
        //Debug.LogFormat("velocity {0}", this.rb.velocity.magnitude);
    }
    Vector3 lastPos;
    //float lastSync = 0;
    private void LateUpdate()
    {
        if (character == null || !isActive)
            return;
        // LateUpdate一般用来处理跟随。

        // 刚体当前帧与上一帧的位置偏移。根据便宜计算速度
        Vector3 offset = this.rb.transform.position - lastPos;
        this.speed = (int)(offset.magnitude * 100f / Time.deltaTime);
        //Debug.LogFormat("LateUpdate velocity {0} : {1}", this.rb.velocity.magnitude, this.speed);
        // 更新之前的位置为当前位置，用于下一帧继续计算
        this.lastPos = this.rb.transform.position;

        // 刚体和角色距离>50，重设角色位置并重置Entity状态。重置状态不理解
        if ((GameObjectTool.WorldToLogic(this.rb.transform.position) - this.character.position).magnitude > 50)
        {
            this.character.SetPosition(GameObjectTool.WorldToLogic(this.rb.transform.position));
            this.SendEntityEvent(EntityEvent.None);
        }
        // go的位置更新为刚体位置，不明所以
        this.transform.position = this.rb.transform.position;
    }

    void SendEntityEvent(EntityEvent entityEvent)
    {
        if (entityController != null)
            entityController.OnEntityEvent(entityEvent);
        character.OnUpdate(Time.fixedDeltaTime);
        MapService.Instance.SendMapEntitySync(entityEvent, character.EntityData);
    }
}
