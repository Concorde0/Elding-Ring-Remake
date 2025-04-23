using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Animations;

public class WASDWalkController : MonoBehaviour
{
    public AnimationClip walkForward;
    public AnimationClip walkBackward;
    public AnimationClip walkLeft;
    public AnimationClip walkRight;

    private PlayableGraph _graph;
    private AnimationMixerPlayable _mixer;

    void Start()
    {
        _graph = PlayableGraph.Create("WASDWalkGraph");
        _graph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);

        // 1. 创建 Mixer，4 个输入
        _mixer = AnimationMixerPlayable.Create(_graph, 4);

        // 2. 连接 4 段动画到 Mixer 的 4 个通道
        var clips = new[] { walkForward, walkBackward, walkLeft, walkRight };
        for (int i = 0; i < clips.Length; i++)
        {
            var clipPlayable = AnimationClipPlayable.Create(_graph, clips[i]);
            clipPlayable.SetTime(0f);              // 从头开始
            _graph.Connect(clipPlayable, 0, _mixer, i);
            _mixer.SetInputWeight(i, 0f);         // 初始全关
        }

        // 3. 输出到 Animator
        var output = AnimationPlayableOutput.Create(_graph, "AnimOut", GetComponent<Animator>());
        output.SetSourcePlayable(_mixer);

        _graph.Play();
    }

    void Update()
    {
        // 读取 WASD（或箭头）输入
        float v = Input.GetAxisRaw("Vertical");    // W=+1, S=-1
        float h = Input.GetAxisRaw("Horizontal");  // D=+1, A=-1

        // 计算每段动画应有的权重
        float wF = Mathf.Max(0,  v);   // 前
        float wB = Mathf.Max(0, -v);   // 后
        float wR = Mathf.Max(0,  h);   // 右
        float wL = Mathf.Max(0, -h);   // 左

        // 应用权重
        _mixer.SetInputWeight(0, wF);
        _mixer.SetInputWeight(1, wB);
        _mixer.SetInputWeight(2, wL);
        _mixer.SetInputWeight(3, wR);

        // （可选）归一化：保证 sum=1 时做平滑 Blend
        float sum = wF + wB + wL + wR;
        if (sum > 1f)
        {
            _mixer.SetInputWeight(0, wF / sum);
            _mixer.SetInputWeight(1, wB / sum);
            _mixer.SetInputWeight(2, wL / sum);
            _mixer.SetInputWeight(3, wR / sum);
        }
    }

    void OnDisable()
    {
        _graph.Destroy();
    }
}
