// Client phải tự điều khiển chuỗi Subsystem phức tạp
AudioCodec audio = new AudioCodec();
VideoCodec video = new VideoCodec();
BitrateBuffer buffer = new BitrateBuffer();

audio.FixAudio();
video.FixVideo();
buffer.ReadBuffer();