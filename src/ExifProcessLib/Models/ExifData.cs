using System;
using System.Collections.Generic;
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
                            case ExifTag.Compression:
                                var compressionTag = (ExifCompression) (UInt16.Parse(obj.Value));
                                return GetDisplayString(compressionTag);

                            case ExifTag.LightSource:
                                var lightSourceTag = (ExifLightSource) (UInt16.Parse(obj.Value));
                                return GetDisplayString(lightSourceTag);

                            case ExifTag.PhotometricInterpretation:
                                var photometricInterpretationTag = (ExifPhotometricInterpretation) (UInt16.Parse(obj.Value));
                                return GetDisplayString(photometricInterpretationTag);

                            case ExifTag.Orientation:
                                var orientationTag = (ExifOrientation) (UInt16.Parse(obj.Value));
                                return GetDisplayString(orientationTag);

                            case ExifTag.PlanarConfiguration:
                                var planarConfigurationTag = (ExifPlanarConfiguration) (UInt16.Parse(obj.Value));
                                return GetDisplayString(planarConfigurationTag);

                            case ExifTag.YCbCrSubSampling:
                                var subSamplingValues = obj.Value.Split(',').Select(x => x.Trim()).Select(UInt16.Parse).ToArray();
                                if (subSamplingValues[0] == 1 && subSamplingValues[1] == 1)
                                {
                                    return "YCbCr 4:4:4";
                                }
                                if (subSamplingValues[0] == 1 && subSamplingValues[1] == 2)
                                {
                                    return "YCbCr 4:4:0";
                                }
                                if (subSamplingValues[0] == 1 && subSamplingValues[1] == 4)
                                {
                                    return "YCbCr 4:4:1";
                                }
                                if (subSamplingValues[0] == 2 && subSamplingValues[1] == 1)
                                {
                                    return "YCbCr 4:2:2";
                                }
                                if (subSamplingValues[0] == 2 && subSamplingValues[1] == 2)
                                {
                                    return "YCbCr 4:2:0";
                                }
                                if (subSamplingValues[0] == 2 && subSamplingValues[1] == 4)
                                {
                                    return "YCbCr 4:2:1";
                                }
                                if (subSamplingValues[0] == 4 && subSamplingValues[1] == 1)
                                {
                                    return "YCbCr 4:1:1";
                                }
                                if (subSamplingValues[0] == 4 && subSamplingValues[1] == 2)
                                {
                                    return "YCbCr 4:1:0";
                                }
                                
                                return obj.Value;

                            case ExifTag.YCbCrPositioning:
                                var yCbCrPositioningTag = (ExifYCbCrPositioning)(UInt16.Parse(obj.Value));
                                return GetDisplayString(yCbCrPositioningTag);

                            case ExifTag.XResolution:
                            case ExifTag.YResolution:
                            case ExifTag.Gamma:
                            case ExifTag.CompressedBitsPerPixel:
                            case ExifTag.ExposureTime:
                            case ExifTag.FNumber:
                                var rationalVals = obj.Value.Split('/').Select(x => x.Trim()).Select(UInt32.Parse).ToArray();
                                return String.Format("{0:0.00}", (decimal)rationalVals[0] / rationalVals[1]);

                            case ExifTag.WhitePoint:
                                var whitePointVals = obj.Value.Split(',').SelectMany(x => x.Split('/')).Select(x => x.Trim()).Select(UInt32.Parse).ToArray();
                                var whitePoint1 = (decimal)whitePointVals[0] / whitePointVals[1];
                                var whitePoint2 = (decimal)whitePointVals[2] / whitePointVals[3];
                                return String.Format("{0:0.00}, {1:0.00}", whitePoint1, whitePoint2);

                            case ExifTag.YCbCrCoefficients:
                                var yCbCrVals = obj.Value.Split(',').SelectMany(x => x.Split('/')).Select(x => x.Trim()).Select(UInt32.Parse).ToArray();
                                var yCbCr1 = (decimal)yCbCrVals[0] / yCbCrVals[1];
                                var yCbCr2 = (decimal)yCbCrVals[2] / yCbCrVals[3];
                                var yCbCr3 = (decimal)yCbCrVals[4] / yCbCrVals[5];
                                return String.Format("{0:0.00}, {1:0.00}, {2:0.00}", yCbCr1, yCbCr2, yCbCr3);

                            case ExifTag.PrimaryChromaticities:
                            case ExifTag.ReferenceBlackWhite:
                                var primaryChromaticitiesVals = obj.Value.Split(',').SelectMany(x => x.Split('/')).Select(x => x.Trim()).Select(UInt32.Parse).ToArray();
                                var primaryChromaticity1 = (decimal)primaryChromaticitiesVals[0] / primaryChromaticitiesVals[1];
                                var primaryChromaticity2 = (decimal)primaryChromaticitiesVals[2] / primaryChromaticitiesVals[3];
                                var primaryChromaticity3 = (decimal)primaryChromaticitiesVals[4] / primaryChromaticitiesVals[5];
                                var primaryChromaticity4 = (decimal)primaryChromaticitiesVals[6] / primaryChromaticitiesVals[7];
                                var primaryChromaticity5 = (decimal)primaryChromaticitiesVals[8] / primaryChromaticitiesVals[9];
                                var primaryChromaticity6 = (decimal)primaryChromaticitiesVals[10] / primaryChromaticitiesVals[11];
                                return String.Format("{0:0.00}, {1:0.00}, {2:0.00}, {3:0.00}, {4:0.00}, {5:0.00}", primaryChromaticity1, primaryChromaticity2, primaryChromaticity3, primaryChromaticity4, primaryChromaticity5, primaryChromaticity6);

                            case ExifTag.ResolutionUnit:
                                var resolutionUnitTag = (ExifResolutionUnit)(UInt16.Parse(obj.Value));
                                return GetDisplayString(resolutionUnitTag);

                            case ExifTag.ColorSpace:
                                var colorSpaceTag = (ExifColorSpace)(UInt16.Parse(obj.Value));
                                return GetDisplayString(colorSpaceTag);

                            case ExifTag.ComponentConfiguration:
                                var componentConfiguration = (ExifComponentConfiguration)(UInt16.Parse(obj.Value));
                                return GetDisplayString(componentConfiguration);

                            // Up to http://www.cipa.jp/std/documents/e/DC-008-2012_E.pdf, p50
                            // http://www.sno.phy.queensu.ca/~phil/exiftool/TagNames/EXIF.html also useful

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
                            case GPSTag.GPSHpositioningError:
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

                            case GPSTag.GPSDifferential:
                                return obj.Value.Equals("0")
                                    ? "No differential correction applied"
                                    : (obj.Value.Equals("1") ? "Differential correction applied" : obj.Value);

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

        private static string GetDisplayString(ExifCompression tag)
        {
            return ExifCompressionStrings.ContainsKey(tag) ? ExifCompressionStrings[tag] : tag.ToString();
        }

        private static string GetDisplayString(ExifLightSource tag)
        {
            return ExifLightSourceStrings.ContainsKey(tag) ? ExifLightSourceStrings[tag] : tag.ToString();
        }

        private static string GetDisplayString(ExifPhotometricInterpretation tag)
        {
            return ExifPhotometricInterpretationStrings.ContainsKey(tag) ? ExifPhotometricInterpretationStrings[tag] : tag.ToString();
        }

        private static string GetDisplayString(ExifOrientation tag)
        {
            return ExifOrientationStrings.ContainsKey(tag) ? ExifOrientationStrings[tag] : tag.ToString();
        }

        private static string GetDisplayString(ExifPlanarConfiguration tag)
        {
            return ExifPlanarConfigurationStrings.ContainsKey(tag) ? ExifPlanarConfigurationStrings[tag] : tag.ToString();
        }

        private static string GetDisplayString(ExifYCbCrPositioning tag)
        {
            return ExifYCbCrPositioningStrings.ContainsKey(tag) ? ExifYCbCrPositioningStrings[tag] : tag.ToString();
        }

        private static string GetDisplayString(ExifResolutionUnit tag)
        {
            return ExifResolutionUnitStrings.ContainsKey(tag) ? ExifResolutionUnitStrings[tag] : tag.ToString();
        }

        private static string GetDisplayString(ExifColorSpace tag)
        {
            return ExifColorSpaceStrings.ContainsKey(tag) ? ExifColorSpaceStrings[tag] : tag.ToString();
        }

        private static string GetDisplayString(ExifComponentConfiguration tag)
        {
            return ExifComponentConfigurationStrings.ContainsKey(tag) ? ExifComponentConfigurationStrings[tag] : tag.ToString();
        }

        private static readonly Dictionary<ExifCompression, string> ExifCompressionStrings = new Dictionary<ExifCompression, string>
        {
            {ExifCompression.Uncompressed,                                          "Uncompressed"},
            {ExifCompression.CCITT1D,                                               "CCITT 1D"},
            {ExifCompression.T4Group3Fax,                                           "T4/Group 3 Fax"},
            {ExifCompression.T6Group4Fax,                                           "T6/Group 4 Fax"},
            {ExifCompression.LZW,                                                   "LZW"},
            {ExifCompression.JPEGOldStyle,                                          "JPEG (old-style)"},
            {ExifCompression.JPEG,                                                  "JPEG"},
            {ExifCompression.AdobeDeflate,                                          "Adobe Deflate"},
            {ExifCompression.JBIGBW,                                                "JBIG B&W"},
            {ExifCompression.JBIGColor,                                             "JBIG Color"},
            {ExifCompression.JPEG2,                                                 "JPEG"},
            {ExifCompression.Kodak262,                                              "Kodak 262"},
            {ExifCompression.Next,                                                  "Next"},
            {ExifCompression.SonyARWCompressed,                                     "Sony ARW Compressed"},
            {ExifCompression.PackedRAW,                                             "Packed RAW"},
            {ExifCompression.SamsungSRWCompressed,                                  "Samsung SRW Compressed"},
            {ExifCompression.CCIRLEW,                                               "CCIRLEW"},
            {ExifCompression.SamsungSRWCompressed2,                                 "Samsung SRW Compressed 2"},
            {ExifCompression.PackBits,                                              "PackBits"},
            {ExifCompression.Thunderscan,                                           "Thunderscan"},
            {ExifCompression.KodakKDCCompressed,                                    "Kodak KDC Compressed"},
            {ExifCompression.IT8CTPAD,                                              "IT8CTPAD"},
            {ExifCompression.IT8LW,                                                 "IT8LW"},
            {ExifCompression.IT8MP,                                                 "IT8MP"},
            {ExifCompression.IT8BL,                                                 "IT8BL"},
            {ExifCompression.PixarFilm,                                             "PixarFilm"},
            {ExifCompression.PixarLog,                                              "PixarLog"},
            {ExifCompression.Deflate,                                               "Deflate"},
            {ExifCompression.DCS,                                                   "DCS"},
            {ExifCompression.JBIG,                                                  "JBIG"},
            {ExifCompression.SGILog,                                                "SGILog"},
            {ExifCompression.SGILog24,                                              "SGILog24"},
            {ExifCompression.JPEG2000,                                              "JPEG 2000"},
            {ExifCompression.NikonNEFCompressed,                                    "Nikon NEF Compressed"},
            {ExifCompression.JBIG2TIFFFX,                                           "JBIG2 TIFF FX"},
            {ExifCompression.MicrosoftDocumentImagingMDIBinaryLevelCodec,           "Microsoft Document Imaging (MDI) Binary Level Codec"},
            {ExifCompression.MicrosoftDocumentImagingMDIProgressiveTransformCodec,  "Microsoft Document Imaging (MDI) Progressive Transform Codec"},
            {ExifCompression.MicrosoftDocumentImagingMDIVector,                     "Microsoft Document Imaging (MDI) Vector"},
            {ExifCompression.LossyJPEG,                                             "Lossy JPEG"},
            {ExifCompression.KodakDCRCompressed,                                    "Kodak DCR Compressed"},
            {ExifCompression.PentaxPEFCompressed,                                   "Pentax PEF Compressed"}
        };

        private static readonly Dictionary<ExifLightSource, string> ExifLightSourceStrings = new Dictionary<ExifLightSource, string>
        {
            {ExifLightSource.Unknown,               "Unknown"},
            {ExifLightSource.Daylight,              "Daylight"},
            {ExifLightSource.Fluorescent,           "Fluorescent"},
            {ExifLightSource.TungstenIncandescent,  "Tungsten (Incandescent)"},
            {ExifLightSource.Flash,                 "Flash"},
            {ExifLightSource.FineWeather,           "Fine Weather"},
            {ExifLightSource.Cloudy,                "Cloudy"},
            {ExifLightSource.Shade,                 "Shade"},
            {ExifLightSource.DaylightFluorescent,   "Daylight Fluorescent"},
            {ExifLightSource.DayWhiteFluorescent,   "Day White Fluorescent"},
            {ExifLightSource.CoolWhiteFluorescent,  "Cool White Fluorescent"},
            {ExifLightSource.WhiteFluorescent,      "White Fluorescent"},
            {ExifLightSource.WarmWhiteFluorescent,  "Warm White Fluorescent"},
            {ExifLightSource.StandardLightA,        "Standard Light A"},
            {ExifLightSource.StandardLightB,        "Standard Light B"},
            {ExifLightSource.StandardLightC,        "Standard Light C"},
            {ExifLightSource.D55,                   "D55"},
            {ExifLightSource.D65,                   "D65"},
            {ExifLightSource.D75,                   "D75"},
            {ExifLightSource.D50,                   "D50"},
            {ExifLightSource.ISOStudioTungsten,     "ISO Studio Tungsten"},
	        {ExifLightSource.Other,                 "Other"}
        };

        private static readonly Dictionary<ExifPhotometricInterpretation, string> ExifPhotometricInterpretationStrings = new Dictionary<ExifPhotometricInterpretation, string>
        {
            {ExifPhotometricInterpretation.WhiteIsZero,         "WhiteIsZero"},    
            {ExifPhotometricInterpretation.BlackIsZero,         "BlackIsZero"},
            {ExifPhotometricInterpretation.RGB,                 "RGB"},
            {ExifPhotometricInterpretation.RGBPalette,          "RGB Palette"},
            {ExifPhotometricInterpretation.TransparencyMask,    "Transparency Mask"},
            {ExifPhotometricInterpretation.CMYK,                "CMYK"},
            {ExifPhotometricInterpretation.YCbCr,               "YCbCr"},
            {ExifPhotometricInterpretation.CIELab,              "CIELab"},
            {ExifPhotometricInterpretation.ICCLab,              "ICCLab"},
            {ExifPhotometricInterpretation.ITULab,              "ITULab"},
            {ExifPhotometricInterpretation.ColorFilterArray,    "Color Filter Array"},
            {ExifPhotometricInterpretation.PixarLogL,           "Pixar LogL"},
            {ExifPhotometricInterpretation.PixarLogLuv,         "Pixar LogLuv"},
            {ExifPhotometricInterpretation.LinearRaw,           "Linear Raw"}
        };

        private static readonly Dictionary<ExifOrientation, string> ExifOrientationStrings = new Dictionary<ExifOrientation, string>
        {
            {ExifOrientation.Horizontal,                    "Horizontal (normal)"},
            {ExifOrientation.MirrorHorizontal,              "Mirror horizontal"},
            {ExifOrientation.Rotate180,                     "Rotate 180°"},
            {ExifOrientation.MirrorVertical,                "Mirror vertical"},
            {ExifOrientation.MirrorHorizontalRotate270CW,   "Mirror horizontal and rotate 270° CW"},
            {ExifOrientation.Rotate90CW,                    "Rotate 90° CW"},
            {ExifOrientation.MirrorHorizontalRotate90CW,    "Mirror horizontal and rotate 90° CW"},
            {ExifOrientation.Rotate270CW,                   "Rotate 270° CW"}
        };

        private static readonly Dictionary<ExifPlanarConfiguration, string> ExifPlanarConfigurationStrings = new Dictionary<ExifPlanarConfiguration, string>
        {
            {ExifPlanarConfiguration.Chunky,    "Chunky"},
            {ExifPlanarConfiguration.Planar,    "Planar"}
        };

        private static readonly Dictionary<ExifYCbCrPositioning, string> ExifYCbCrPositioningStrings = new Dictionary<ExifYCbCrPositioning, string>
        {
            {ExifYCbCrPositioning.Centered, "Centered"},
            {ExifYCbCrPositioning.CoSited,  "Co-sited"}
        };

        private static readonly Dictionary<ExifResolutionUnit, string> ExifResolutionUnitStrings = new Dictionary<ExifResolutionUnit, string>
        {
            {ExifResolutionUnit.None,   "None"},
            {ExifResolutionUnit.Inches, "inches"},
            {ExifResolutionUnit.Cm,     "cm"}
        };

        private static readonly Dictionary<ExifColorSpace, string> ExifColorSpaceStrings = new Dictionary<ExifColorSpace, string>
        {
            {ExifColorSpace.SRGB,           "sRGB"},
            {ExifColorSpace.AdobeRGB,       "Adobe RGB"},
            {ExifColorSpace.WideGamutRGB,   "Wide Gamut RGB"},
            {ExifColorSpace.ICCProfile,     "ICC Profile"},
            {ExifColorSpace.Uncalibrated,   "Uncalibrated"}
        };

        private static readonly Dictionary<ExifComponentConfiguration, string> ExifComponentConfigurationStrings = new Dictionary<ExifComponentConfiguration, string>
        {
            {ExifComponentConfiguration.None,   "-"},
            {ExifComponentConfiguration.Y,      "Y"},
            {ExifComponentConfiguration.Cb,     "Cb"},
            {ExifComponentConfiguration.Cr,     "Cr"},
            {ExifComponentConfiguration.R,      "R"},
            {ExifComponentConfiguration.G,      "G"},
            {ExifComponentConfiguration.B,      "B"},
        };
    }
}
