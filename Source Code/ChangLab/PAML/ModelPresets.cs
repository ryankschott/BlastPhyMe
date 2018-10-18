using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ChangLab.Common;

namespace ChangLab.PAML.CodeML
{
    public class ModelPreset : ReferenceItem2<ModelPresets>
    {
        public int Model { get; set; }
        public List<int> NSSites { get; set; }
        public int NCatG { get; set; }
        public bool NCatGFixed { get; set; }
        public RangeWithInterval Omega { get; set; }

        public ModelPreset()
        {
            SetDefaultValues();
        }

        public ModelPreset(ReferenceItem2<ModelPresets> ReferenceItem)
            : base(ReferenceItem.ID, ReferenceItem.Key, ReferenceItem.Name, ReferenceItem.ShortName, ReferenceItem.Rank)
        {
            SetDefaultValues();
        }

        private void SetDefaultValues()
        {
            Omega = new RangeWithInterval(0.00, 2.00, 1.00, false);
            NCatGFixed = true;
        }

        private static List<ModelPreset> _all = null;
        public static List<ModelPreset> All
        {
            get
            {
                if (_all == null)
                {
                    _all = new List<ModelPreset>();
                    List<ReferenceItem2<ModelPresets>> referenceList = ModelPresetCollection.Instance.All;

                    _all.Add(new ModelPreset(referenceList.First(m => m.Key == ModelPresets.Model0))
                    {
                        Model = 0,
                        NSSites = (new int[] { 0, 1, 2, 3, 7, 8 }).ToList(),
                        NCatG = 10,
                        NCatGFixed = false
                    });

                    _all.Add(new ModelPreset(referenceList.First(m => m.Key == ModelPresets.Model2a))
                    {
                        Model = 0,
                        NSSites = (new int[] { 22 }).ToList(),
                        NCatG = 10
                    });

                    _all.Add(new ModelPreset(referenceList.First(m => m.Key == ModelPresets.Model8a))
                    {
                        Model = 0,
                        NSSites = (new int[] { 8 }).ToList(),
                        NCatG = 10,
                        NCatGFixed = false,
                        Omega = new RangeWithInterval(1.00, 1.00, 1.00, true)
                    });

                    _all.Add(new ModelPreset(referenceList.First(m => m.Key == ModelPresets.Branch))
                    {
                        Model = 2,
                        NSSites = (new int[] { 0 }).ToList(),
                        NCatG = 3
                    });

                    _all.Add(new ModelPreset(referenceList.First(m => m.Key == ModelPresets.BranchNull))
                    {
                        Model = 2,
                        NSSites = (new int[] { 0 }).ToList(),
                        NCatG = 3,
                        Omega = new RangeWithInterval(1.00, 1.00, 1.00, true)
                    });

                    _all.Add(new ModelPreset(referenceList.First(m => m.Key == ModelPresets.BranchSite))
                    {
                        Model = 2,
                        NSSites = (new int[] { 2 }).ToList(),
                        NCatG = 3
                    });

                    _all.Add(new ModelPreset(referenceList.First(m => m.Key == ModelPresets.BranchSiteNull))
                    {
                        Model = 2,
                        NSSites = (new int[] { 2 }).ToList(),
                        NCatG = 3,
                        Omega = new RangeWithInterval(1.00, 1.00, 1.00, true)
                    });

                    _all.Add(new ModelPreset(referenceList.First(m => m.Key == ModelPresets.CmC))
                    {
                        Model = 3,
                        NSSites = (new int[] { 2 }).ToList(),
                        NCatG = 3
                    });

                    _all.Add(new ModelPreset(referenceList.First(m => m.Key == ModelPresets.CmCNull))
                    {
                        Model = 3,
                        NSSites = (new int[] { 2 }).ToList(),
                        NCatG = 3,
                        Omega = new RangeWithInterval(1.00, 1.00, 1.00, true)
                    });

                    _all.Add(new ModelPreset(referenceList.First(m => m.Key == ModelPresets.CmD))
                    {
                        Model = 3,
                        NSSites = (new int[] { 3 }).ToList(),
                        NCatG = 3
                    });

                    _all.Add(new ModelPreset(referenceList.First(m => m.Key == ModelPresets.CmDNull))
                    {
                        Model = 3,
                        NSSites = (new int[] { 3 }).ToList(),
                        NCatG = 3,
                        Omega = new RangeWithInterval(1.00, 1.00, 1.00, true)
                    });
                }

                return _all;
            }
        }

        //public static List<ModelPreset> ListAll()
        //{
        //    if (_all == null)
        //    {
        //        _all = new List<ModelPreset>();

        //        _all.Add(new ModelPreset()
        //            {
        //                Name = "Model 0 (Sites)",
        //                ShortName = "Model 0",
        //                Key = "Model0",
        //                Model = 0,
        //                NSSites = (new int[] { 0, 1, 2, 3, 7, 8 }).ToList(),
        //                NCatG = 10,
        //                NCatGFixed = false
        //            });

        //        _all.Add(new ModelPreset()
        //        {
        //            Name = "Model 2a rel",
        //            ShortName = "Model 2a rel",
        //            Key = "Model2a",
        //            Model = 0,
        //            NSSites = (new int[] { 22 }).ToList(),
        //            NCatG = 10
        //        });

        //        _all.Add(new ModelPreset()
        //        {
        //            Name = "Model 8a",
        //            ShortName = "Model 8a",
        //            Key = "Model8a",
        //            Model = 0,
        //            NSSites = (new int[] { 8 }).ToList(),
        //            NCatG = 10,
        //            NCatGFixed = false,
        //            OmegaFixed = true
        //        });

        //        _all.Add(new ModelPreset()
        //        {
        //            Name = "Model 2 (Branch)",
        //            ShortName = "Branch",
        //            Key = "Branch",
        //            Model = 2,
        //            NSSites = (new int[] { 0 }).ToList(),
        //            NCatG = 3
        //        });

        //        _all.Add(new ModelPreset()
        //        {
        //            Name = "Model 2 Null (Branch Null)",
        //            ShortName = "Branch Null",
        //            Key = "BranchNull",
        //            Model = 2,
        //            NSSites = (new int[] { 0 }).ToList(),
        //            NCatG = 3,
        //            OmegaFixed = true
        //        });

        //        _all.Add(new ModelPreset()
        //        {
        //            Name = "Model 2 Alt (Branch-Site)",
        //            ShortName = "Branch-Site",
        //            Key = "BranchSite",
        //            Model = 2,
        //            NSSites = (new int[] { 2 }).ToList(),
        //            NCatG = 3
        //        });

        //        _all.Add(new ModelPreset()
        //        {
        //            Name = "Model 2 Alt Null (Branch-Site Null)",
        //            ShortName = "Branch-Site Null",
        //            Key = "BranchSiteNull",
        //            Model = 2,
        //            NSSites = (new int[] { 2 }).ToList(),
        //            NCatG = 3,
        //            OmegaFixed = true
        //        });

        //        _all.Add(new ModelPreset()
        //        {
        //            Name = "Model 3 (Clade model C)",
        //            ShortName = "Clade model C",
        //            Key = "CmC",
        //            Model = 3,
        //            NSSites = (new int[] { 2 }).ToList(),
        //            NCatG = 3
        //        });

        //        _all.Add(new ModelPreset()
        //        {
        //            Name = "Model 3 Null (Clade model C Null)",
        //            ShortName = "Clade model C Null",
        //            Key = "CmCNull",
        //            Model = 3,
        //            NSSites = (new int[] { 2 }).ToList(),
        //            NCatG = 3,
        //            OmegaFixed = true
        //        });
        //    }

        //    return _all;
        //}

        public static ModelPreset Derive(AnalysisConfiguration Configuration)
        {
            if (Configuration.ModelPresetID != 0)
            {
                return All.First(m => m.Key == ModelPresetCollection.KeyByID(Configuration.ModelPresetID));
            }
            else
            {
                switch (Configuration.Model)
                {
                    case 0:
                        if (Configuration.NSSites.Contains(22))
                        { return All.First(m => m.Key == ModelPresets.Model2a); }
                        else if (Configuration.NSSites.Contains(8)
                                    && Configuration.NSSites.Count == 1
                                    && Configuration.FixedOmega)
                        { return All.First(m => m.Key == ModelPresets.Model8a); }
                        else
                        { return All.First(m => m.Key == ModelPresets.Model0); }
                    case 2:
                        if (Configuration.NSSites.Only(0))
                        { return All.First(m => m.Key == (Configuration.FixedOmega ? ModelPresets.BranchNull : ModelPresets.Branch)); }
                        else
                        { return All.First(m => m.Key == (Configuration.FixedOmega ? ModelPresets.BranchSiteNull : ModelPresets.BranchSite)); }
                    case 3:
                        if (Configuration.NSSites.Contains(2))
                        { return All.First(m => m.Key == (Configuration.FixedOmega ? ModelPresets.CmCNull : ModelPresets.CmC)); }
                        else if (Configuration.NSSites.Contains(3))
                        { return All.First(m => m.Key == (Configuration.FixedOmega ? ModelPresets.CmDNull : ModelPresets.CmD)); }
                        else
                        { return null; }
                    default:
                        return null;
                }
            }
        }

        public static ModelPreset Derive(ModelPresets Key)
        {
            return All.First(m => m.Key == Key);
        }

        public static ModelPreset Derive(int ID)
        {
            return All.First(m => m.Key == ModelPresetCollection.KeyByID(ID));
        }

        public static ModelPreset Derive(string Key)
        {
            return All.First(m => m.Key.ToString() == Key);
        }
    }

    public class ModelPresetCollection : ReferenceItemCollection2<ModelPresets>
    {

    }

    [ChangLab.Common.ReferenceItemAttribute(ListProcedure = "PAML.ModelPreset_List")]
    public enum ModelPresets
    {
        Model0 = 1,
        Model2a = 2,
        Model8a = 3,
        Branch = 4,
        BranchNull = 5,
        BranchSite = 6,
        BranchSiteNull = 7,
        CmC = 8,
        CmCNull = 9,
        CmD = 10,
        CmDNull = 11
    }
}
