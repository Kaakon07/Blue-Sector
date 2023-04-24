using UnityEngine;

public class FasterOcean : MonoBehaviour
{
    private const int NB_WAVE = 3;
    private const int NB_INTERACTIONS = 64 / 8;

    public Material ocean;
    public Light sun;

    private static readonly Wave[] waves =
    {
        new Wave(99, 1.0f, .4f * 1.4f, 0.9f, new Vector2(1.0f,  0.2f)),
        new Wave(60, 1.2f, .4f * 0.8f, 0.5f, new Vector2(1.0f,  3.0f)),
        new Wave(20, 3.5f, .4f * 0.4f, 0.8f, new Vector2(2.0f,  4.0f))
    };
    private readonly Vector4[] interactions = new Vector4[NB_INTERACTIONS];
    private int interaction_id = 0;

    void Awake()
    {
        Vector4[] v_waves = new Vector4[NB_WAVE];
        Vector4[] v_waves_dir = new Vector4[NB_WAVE];
        for (int i = 0; i < NB_WAVE; i++)
        {
            v_waves[i] = new Vector4(waves[i].frequency, waves[i].amplitude, waves[i].phase, waves[i].sharpness);
            v_waves_dir[i] = new Vector4(waves[i].direction.x, waves[i].direction.y, 0, 0);
        }

        ocean.SetVectorArray("waves_p", v_waves);
        ocean.SetVectorArray("waves_d", v_waves_dir);

        ocean.SetVector("world_light_dir", -sun.transform.forward);
        ocean.SetVector("sun_color", new Vector4(sun.color.r, sun.color.g, sun.color.b, 0.0F));
    }

    private readonly struct Wave
    {
        public float waveLength { get; }
        public float speed { get; }
        public float amplitude { get; }
        public float sharpness { get; }
        public float frequency { get; }
        public float phase { get; }
        public Vector2 direction { get; }

        public Wave(float waveLength, float speed, float amplitude, float sharpness, Vector2 direction)
        {
            this.waveLength = waveLength;
            this.speed = speed;
            this.amplitude = amplitude;
            this.sharpness = sharpness;
            this.direction = direction.normalized;
            frequency = (2 * Mathf.PI) / waveLength;
            phase = frequency * speed;
        }
    }
}
