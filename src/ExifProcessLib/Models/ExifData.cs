using System;
using System.Linq;

namespace ExifProcessLib.Models
{
    public class ExifData
    {
        public ExifTag ExifTag { get; set; }
        public GPSTag GPSTag { get; set; }
        public ExifValueType ValueType { get; set; }
        public IFDType IFDType { get; set; }
        public string Value { get; set; }
    }

    public static class ExifDataExtension
    {
        public static string ToDisplayString(this ExifData obj)
        {
            try
            {
                switch (obj.IFDType)
                {
                    case IFDType.IFD_0_Primary:
                    case IFDType.IFD_1_Thumbnail:
                    case IFDType.IFD_Exif:
                        switch (obj.ExifTag)
                        {
                            default:
                                return obj.Value;
                        }
                        break;

                    case IFDType.IFD_GPS:
                        switch (obj.GPSTag)
                        {
                            case GPSTag.GPSVersionID:
                                return String.Join(".", obj.Value.Split(',').Select(x => x.Trim()));

                            case GPSTag.GPSLatitudeRef:
                            case GPSTag.GPSDestLatitudeRef:
                                return obj.Value.Equals("N") ? "North" : (obj.Value.Equals("S") ? "South" : obj.Value);

                            case GPSTag.GPSLongitudeRef:
                            case GPSTag.GPSDestLongitudeRef:
                                return obj.Value.Equals("E") ? "East" : (obj.Value.Equals("W") ? "West" : obj.Value);

                            case GPSTag.GPSLatitude:
                            case GPSTag.GPSLongitude:
                            case GPSTag.GPSDestLatitude:
                            case GPSTag.GPSDestLongitude:
                                var vals1 = obj.Value.Split(',').Select(x => x.Trim()).ToArray();
                                var degVals = vals1[0].Split('/').Select(x => x.Trim()).Select(x => Convert.ToInt32(x)).ToArray();
                                var minVals = vals1[1].Split('/').Select(x => x.Trim()).Select(x => Convert.ToInt32(x)).ToArray();
                                var secVals = vals1[2].Split('/').Select(x => x.Trim()).Select(x => Convert.ToInt32(x)).ToArray();
                                var deg = degVals[0]/degVals[1];
                                var min = minVals[0]/minVals[1];
                                var sec = secVals[0]/secVals[1];
                                return String.Format("{0}° {1}' {2}\"", deg, min, sec);

                            case GPSTag.GPSAltitudeRef:
                                return obj.Value.Equals("0")
                                    ? "Above Sea Level"
                                    : (obj.Value.Equals("1") ? "Below Sea Level" : obj.Value);

                            case GPSTag.GPSAltitude:
                                var vals2 = obj.Value.Split('/').Select(x => x.Trim()).Select(x => Convert.ToInt32(x)).ToArray();
                                return String.Format("{0:0.00} m", (decimal) vals2[0]/vals2[1]);

                            case GPSTag.GPSTimeStamp:
                                var vals3 = obj.Value.Split(',').Select(x => x.Trim()).ToArray();
                                var hrVals = vals3[0].Split('/').Select(x => x.Trim()).Select(x => Convert.ToInt32(x)).ToArray();
                                var mnVals = vals3[1].Split('/').Select(x => x.Trim()).Select(x => Convert.ToInt32(x)).ToArray();
                                var scVals = vals3[2].Split('/').Select(x => x.Trim()).Select(x => Convert.ToInt32(x)).ToArray();
                                var hr = hrVals[0]/hrVals[1];
                                var mn = mnVals[0]/mnVals[1];
                                var sc = scVals[0]/scVals[1];
                                return String.Format("{0}:{1}:{2} UTC", hr, mn, sc);

                            case GPSTag.GPSStatus:
                                return obj.Value.Equals("A")
                                    ? "Measurement in progress"
                                    : (obj.Value.Equals("V") ? "Measurement interrupted" : obj.Value);

                            case GPSTag.GPSMeasureMode:
                                return obj.Value.Equals("2")
                                    ? "2D measurement"
                                    : (obj.Value.Equals("3") ? "3D measurement" : obj.Value);

                            case GPSTag.GPSSpeedRef:
                                return obj.Value.Equals("K")
                                    ? "Kilometres per hour"
                                    : (obj.Value.Equals("M")
                                        ? "Miles per hour"
                                        : (obj.Value.Equals("N") ? "Knots" : obj.Value));

                            case GPSTag.GPSSpeed:
                            case GPSTag.GPSDestDistance:
                                var vals4 = obj.Value.Split('/').Select(x => x.Trim()).Select(x => Convert.ToInt32(x)).ToArray();
                                return String.Format("{0:0.00}", (decimal)vals4[0] / vals4[1]);

                            case GPSTag.GPSTrackRef:
                            case GPSTag.GPSImgDirectionRef:
                            case GPSTag.GPSDestBearingRef:
                                return obj.Value.Equals("T")
                                    ? "True"
                                    : (obj.Value.Equals("M") ? "Magnetic" : obj.Value);

                            case GPSTag.GPSTrack:
                            case GPSTag.GPSImgDirection:
                            case GPSTag.GPSDestBearing:
                                var vals5 = obj.Value.Split('/').Select(x => x.Trim()).Select(x => Convert.ToInt32(x)).ToArray();
                                return String.Format("{0:0.00}", (decimal)vals5[0] / vals5[1]);

                            case GPSTag.GPSDestDistanceRef:
                                return obj.Value.Equals("K")
                                    ? "Kilometres"
                                    : (obj.Value.Equals("M")
                                        ? "Miles"
                                        : (obj.Value.Equals("N") ? "Nautical miles" : obj.Value));

                            default:
                                return obj.Value;
                        }
                        break;

                    default:
                        return obj.Value;
                }
            }
            catch
            {
                return obj.Value;
            }
        }
    }
}
