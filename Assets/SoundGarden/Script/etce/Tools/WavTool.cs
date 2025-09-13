using System;
using System.IO;

public class WavTool
{
    // Audio �����͸� WAV �������� ��ȯ�ϴ� �Լ�
    public static byte[] ConvertToWav(float[] samples, int channels, int sampleRate)
    {
        MemoryStream stream = new MemoryStream();

        // WAV ���� ��� �ۼ�
        int byteRate = sampleRate * channels * 2; // 16-bit PCM
        stream.Write(System.Text.Encoding.UTF8.GetBytes("RIFF"), 0, 4);
        stream.Write(BitConverter.GetBytes(36 + samples.Length * 2), 0, 4);
        stream.Write(System.Text.Encoding.UTF8.GetBytes("WAVE"), 0, 4);
        stream.Write(System.Text.Encoding.UTF8.GetBytes("fmt "), 0, 4);
        stream.Write(BitConverter.GetBytes(16), 0, 4); // SubChunk1Size (16 for PCM)
        stream.Write(BitConverter.GetBytes((short)1), 0, 2); // AudioFormat (1 for PCM)
        stream.Write(BitConverter.GetBytes((short)channels), 0, 2);
        stream.Write(BitConverter.GetBytes(sampleRate), 0, 4);
        stream.Write(BitConverter.GetBytes(byteRate), 0, 4);
        stream.Write(BitConverter.GetBytes((short)(channels * 2)), 0, 2); // BlockAlign
        stream.Write(BitConverter.GetBytes((short)16), 0, 2); // BitsPerSample

        // ������ ���� ûũ �ۼ�
        stream.Write(System.Text.Encoding.UTF8.GetBytes("data"), 0, 4);
        stream.Write(BitConverter.GetBytes(samples.Length * 2), 0, 4);

        // ���� �����͸� 16-bit PCM���� ��ȯ
        for (int i = 0; i < samples.Length; i++)
        {
            short sample = (short)(samples[i] * short.MaxValue);
            stream.Write(BitConverter.GetBytes(sample), 0, 2);
        }

        return stream.ToArray();
    }
}
