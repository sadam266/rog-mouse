namespace GHelper.USB
{
    public abstract class Device : IDisposable
    {
        protected ushort _vendorId;
        protected ushort _productId;
        protected IUsbProvider _usbProvider;

        protected Device(ushort vendorId, ushort productId)
        {
            _vendorId = vendorId;
            _productId = productId;
        }

        public ushort VendorID() => _vendorId;
        public ushort ProductID() => _productId;

        public abstract void SetProvider();

        public virtual void Dispose()
        {
            _usbProvider?.Dispose();
        }

        protected long MeasuredIO(Action io, byte[] data)
        {
            var watch = System.Diagnostics.Stopwatch.StartNew();
            io();
            watch.Stop();
            return watch.ElapsedMilliseconds;
        }

        protected void Write(byte[] data)
        {
            _usbProvider?.Write(data);
        }

        protected void Read(byte[] data)
        {
            _usbProvider?.Read(data);
        }
    }

    public interface IUsbProvider : IDisposable
    {
        void Write(byte[] data);
        void Read(byte[] data);
    }

    public class WindowsUsbProvider : IUsbProvider
    {
        private HidSharp.HidStream _stream;

        public WindowsUsbProvider(ushort vendorId, ushort productId, string path, int timeout)
        {
            var device = HidSharp.DeviceList.Local.GetHidDevices(vendorId, productId)
                .FirstOrDefault(x => x.DevicePath.Contains(path));
            if (device != null)
            {
                _stream = device.Open();
                _stream.ReadTimeout = timeout;
                _stream.WriteTimeout = timeout;
            }
        }

        public void Write(byte[] data)
        {
            _stream?.Write(data);
        }

        public void Read(byte[] data)
        {
            _stream?.Read(data);
        }

        public void Dispose()
        {
            _stream?.Dispose();
        }
    }
}
