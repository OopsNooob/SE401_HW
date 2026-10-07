// Client phải tự điều khiển chuỗi Subsystem phức tạp
AudioCodec audio = new AudioCodec();
VideoCodec video = new VideoCodec();
BitrateBuffer buffer = new BitrateBuffer();

audio.FixAudio();
video.FixVideo();
buffer.ReadBuffer();

// Client chỉ tương tác qua 1 giao diện duy nhất
VideoConverterConverter converter = new VideoConverter();
converter.ConvertVideo("movie.mp4"); // Xử lý ngầm bên trong
