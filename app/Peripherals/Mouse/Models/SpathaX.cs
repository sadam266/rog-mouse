using static RogMouse.Peripherals.Mouse.LightingMode;
using static RogMouse.Peripherals.Mouse.LightingZone;
using static RogMouse.Peripherals.Mouse.PollingRate;

namespace RogMouse.Peripherals.Mouse.Models
{
    //SPATHA_WIRELESS
    public class SpathaX : AsusMouse
    {
        public SpathaX() : base(0x0B05, 0x1979, "mi_00", true)
        {
        }

        protected SpathaX(ushort vendorId, bool wireless) : base(0x0B05, vendorId, "mi_00", wireless)
        {
        }

        public override string GetDisplayName()
        {
            return "ROG Spatha X (Wireless)";
        }

        public override PollingRate[] SupportedPollingrates() =>
        [
            PR250Hz,
            PR500Hz,
            PR1000Hz
        ];

        public override bool HasAngleSnapping()
        {
            return true;
        }

        public override int ProfileCount()
        {
            return 5;
        }

        public override int DPIProfileCount()
        {
            return 4;
        }

        public override int MaxDPI()
        {
            return 19_000;
        }

        public override bool HasXYDPI()
        {
            return false;
        }

        public override bool HasDebounceSetting()
        {
            return false;
        }

        public override bool HasLiftOffSetting()
        {
            return true;
        }

        public override bool HasRGB()
        {
            return true;
        }

        public override LightingZone[] SupportedLightingZones() =>
        [
            Logo,
            Scrollwheel,
            Underglow,
            Dock
        ];

        public override bool IsLightingModeSupported(LightingMode lightingMode) =>
            lightingMode is Off
                or Static
                or Breathing
                or ColorCycle
                or React
                or Rainbow
                or Comet
                or BatteryState;

        public override bool IsLightingModeSupportedForZone(LightingMode lm, LightingZone lz) =>
            lz switch
            {
                All =>
                    true,
                Scrollwheel or Logo or Underglow =>
                    lm is Static
                        or Breathing
                        or ColorCycle
                        or React,
                Dock =>
                    lm is Static
                        or Breathing
                        or BatteryState,
                _ => IsLightingModeSupported(lm)
            };

        public override bool HasAutoPowerOff()
        {
            return true;
        }

        public override bool HasAngleTuning()
        {
            return false;
        }

        public override bool HasLowBatteryWarning()
        {
            return true;
        }

        public override bool HasDPIColors()
        {
            return false;
        }
    }

    public class SpathaXWired : SpathaX
    {
        public SpathaXWired() : base(0x1977, false)
        {
        }

        public override string GetDisplayName()
        {
            return "ROG Spatha X (Wired)";
        }
    }
}