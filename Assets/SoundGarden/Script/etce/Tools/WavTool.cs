using System;
using System.IO;

public class WavTool
{
    // Audio 데이터를 WAV 포맷으로 변환하는 함수
    public static byte[] ConvertToWav(float[] samples, int channels, int sampleRate)
    {
        MemoryStream stream = new MemoryStream();

        // WAV 파일 헤더 작성
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

        // 데이터 서브 청크 작성
        stream.Write(System.Text.Encoding.UTF8.GetBytes("data"), 0, 4);
        stream.Write(BitConverter.GetBytes(samples.Length * 2), 0, 4);

        // 샘플 데이터를 16-bit PCM으로 변환
        for (int i = 0; i < samples.Length; i++)
        {
            short sample = (short)(samples[i] * short.MaxValue);
            stream.Write(BitConverter.GetBytes(sample), 0, 2);
        }

        return stream.ToArray();
    }
}
