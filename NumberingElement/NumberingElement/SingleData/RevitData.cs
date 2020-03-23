using Autodesk.Revit.ApplicationServices;
using Autodesk.Revit.DB;
using Autodesk.Revit.DB.Structure;
using Autodesk.Revit.UI;
using Autodesk.Revit.UI.Selection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utility;


namespace SingleData
{
    public class RevitData
    {
        public static RevitData Instance { get { return Singleton.Instance.RevitData; } }

        public UIApplication UIApplication { get; set; }
        public Application Application
        {
            get
            {
                return UIApplication.Application;
            }
        }
        public UIDocument UIDocument
        {
            get
            {
                return UIApplication.ActiveUIDocument;
            }
        }
        private Document document;
        public Document Document
        {
            get
            {
                if (document == null)
                {
                    document = UIDocument.Document;
                }
                return document;
            }
            set
            {
                document = value;
            }
        }
        private View activeView;
        public View ActiveView
        {
            get
            {
                if (activeView == null)
                {
                    activeView = UIDocument.ActiveView;
                }
                return activeView;
            }
            set
            {
                activeView = value;
            }
        }

        private Selection selection;
        public Selection Selection
        {
            get
            {
                if (selection == null)
                {
                    selection = UIDocument.Selection;
                }
                return selection;
            }
            set
            {
                selection = value;
            }
        }
        private Plane activePlane;
        public Plane ActivePlane
        {
            get
            {
                if (activePlane == null)
                {
                    activePlane = Plane.CreateByOriginAndBasis(ActiveView.Origin, ActiveView.RightDirection, ActiveView.UpDirection);
                }
                return activePlane;
            }
            set
            {
                activePlane = value;
            }
        }
        private SketchPlane activeSketchPlane;
        public SketchPlane ActiveSketchPlane
        {
            get
            {
                if (activeSketchPlane == null)
                {
                    activeSketchPlane = SketchPlane.Create(Document, ActivePlane);
                }
                return activeSketchPlane;
            }
            set
            {
                activeSketchPlane = value;
            }
        }
        private Transaction transaction;
        public Transaction Transaction
        {
            get
            {
                if (transaction == null) transaction = new Transaction(Document, "RevitAddin"); return transaction;
            }
            set
            {
                transaction = value;
            }
        }
        //private Transaction transaction;
        //public virtual Transaction Transaction
        //{
        //    get
        //    {
        //        if (transaction == null)
        //        {
        //            transaction = new Transaction(Document, Constant.ConstantValue.AddinName);
        //        }
        //        return transaction;
        //    }
        //    set
        //    {
        //        transaction = value;
        //    }
        //}
        private IEnumerable<Element> instanceElements;
        public IEnumerable<Element> InstanceElements
        {
            get
            {
                if (instanceElements == null)
                {
                    instanceElements = new FilteredElementCollector(Document).WhereElementIsNotElementType();
                }
                return instanceElements;
            }
            set
            {
                instanceElements = value;
            }
        }
        private IEnumerable<Element> typeElements;
        public IEnumerable<Element> TypeElements
        {
            get
            {
                if (typeElements == null)
                {
                    typeElements = new FilteredElementCollector(Document).WhereElementIsElementType();
                }
                return typeElements;
            }
            set
            {
                typeElements = value;
            }
        }
        private IEnumerable<Family> families;
        public IEnumerable<Family> Families
        {
            get
            {
                if (families == null)
                {
                    families = InstanceElements.Where(x => x is Family).Cast<Family>();
                }
                return families;
            }
            set
            {
                families = value;
            }
        }
        private IEnumerable<FamilySymbol> familySymbols;
        public IEnumerable<FamilySymbol> FamilySymbols
        {
            get
            {
                if (familySymbols == null)
                {
                    familySymbols = TypeElements.Where(x => x is FamilySymbol).Cast<FamilySymbol>();
                }
                return familySymbols;
            }
            set
            {
                familySymbols = value;
            }
        }
        private IEnumerable<FamilyInstance> familyInstances;
        public IEnumerable<FamilyInstance> FamilyInstances
        {
            get
            {
                if (familyInstances == null)
                {
                    familyInstances = InstanceElements.Where(x => x is FamilyInstance).Cast<FamilyInstance>();
                }
                return familyInstances;
            }
            set
            {
                familyInstances = value;
            }
        }
        private IEnumerable<FamilyInstance> familyinstancesinview;
        public virtual IEnumerable<FamilyInstance> FamilyInstancesInView
        {
            get
            {
                if (familyinstancesinview == null)
                {
                    familyinstancesinview = InstanceElementsInView.Where(x => x is FamilyInstance).Cast<FamilyInstance>();
                }
                return familyinstancesinview;
            }
        }
        private IEnumerable<MultiReferenceAnnotationType> multirefannotypes;
        public virtual IEnumerable<MultiReferenceAnnotationType> MultiRefAnnoTypes
        {
            get
            {
                if (multirefannotypes == null)
                {
                    multirefannotypes = TypeElements.Where(x => x is MultiReferenceAnnotationType).Cast<MultiReferenceAnnotationType>();
                }
                return multirefannotypes;
            }
        }
        
        private IEnumerable<IndependentTag> independentTags;
        public virtual IEnumerable<IndependentTag> IndependentTags
        {
            get
            {
                if(independentTags == null)
                {
                    independentTags = InstanceElements.Where(x => x is IndependentTag).Cast<IndependentTag>();
                }
                return independentTags;
            }
        }
        private IEnumerable<FamilySymbol> structuralRebarTags;
        public IEnumerable<FamilySymbol> StructuralRebarTags
        {
            get
            {
                if(structuralRebarTags == null)
                {
                    structuralRebarTags = FamilySymbols.Where(x => x.Category.Id.IntegerValue == (int)BuiltInCategory.OST_RebarTags);
                }
                return structuralRebarTags;
            }
        }
        private IEnumerable<DimensionType> dimensionTypes;
        public virtual IEnumerable<DimensionType> DimensionTypes
        {
            get
            {
                if(dimensionTypes == null)
                {
                    dimensionTypes = TypeElements.Where(x => x is DimensionType).Cast<DimensionType>();
                }
                return dimensionTypes;
            }
        }
        private IEnumerable<Element> instanceElementsInView;
        public virtual IEnumerable<Element> InstanceElementsInView
        {
            get
            {
                if (instanceElementsInView == null)
                {
                    instanceElementsInView = new FilteredElementCollector(Document, ActiveView.Id).WhereElementIsNotElementType();
                }
                return instanceElementsInView;
            }
        }
        private IEnumerable<FamilyInstance> columninstancesinview;
        public virtual IEnumerable<FamilyInstance> ColumnInstancesInView
        {
            get
            {
                if (columninstancesinview == null)
                {
                    columninstancesinview = FamilyInstancesInView.Where(x => x.Category.Id.IntegerValue == (int)BuiltInCategory.OST_StructuralColumns);
                }
                return columninstancesinview;
            }
        }
        
        private IEnumerable<Rebar> rebarsinview;
        public virtual IEnumerable<Rebar> RebarsInView
        {
            get
            {
                if (rebarsinview == null)
                {
                    rebarsinview = InstanceElementsInView.Where(x => x is Rebar).Cast<Rebar>();
                }
                return rebarsinview;
            }
        }
        private IEnumerable<FamilyInstance> framinginstancesinview;
        public virtual IEnumerable<FamilyInstance> FramingInstancesInView
        {
            get
            {
                if (framinginstancesinview == null)
                {
                    framinginstancesinview = FamilyInstancesInView.Where(x => x.Category.Id.IntegerValue == (int)BuiltInCategory.OST_StructuralFraming);
                }
                return framinginstancesinview;
            }
        }
        private IEnumerable<FamilyInstance> framings;
        public virtual IEnumerable<FamilyInstance> Framings
        {
            get
            {
                if(framings == null)
                {
                    framings = FamilyInstances.Where(x => x.Category.Id.IntegerValue == (int)BuiltInCategory.OST_StructuralFraming);
                }
                return framings;
            }
        }
        private IEnumerable<FamilyInstance> foundations;
        public virtual IEnumerable<FamilyInstance> Foundations
        {
            get
            {
                if(foundations == null)
                {
                    foundations = FamilyInstances.Where(x => x.Category.Id.IntegerValue == (int)BuiltInCategory.OST_StructuralFoundation);
                }
                return foundations;
            }
        }
        private IEnumerable<Floor> floorinstancesinview;
        public virtual IEnumerable<Floor> FloorInstancesInview
        {
            get
            {
                if (floorinstancesinview == null)
                {
                    floorinstancesinview = InstanceElementsInView.Where(x => x is Floor).Cast<Floor>();
                }
                return floorinstancesinview;
            }
        }
        private IEnumerable<View> views;
        public IEnumerable<View> Views
        {
            get
            {
                if (views == null)
                {
                    views = InstanceElements.Where(x => x is View).Cast<View>();
                }
                return views;
            }
            set
            {
                views = value;
            }
        }
        private IEnumerable<Workset> userWorksets;
        public IEnumerable<Workset> UserWorksets
        {
            get
            {
                if (userWorksets == null)
                {
                    userWorksets = new FilteredWorksetCollector(Document).OfKind(WorksetKind.UserWorkset);
                }
                return userWorksets;
            }
            set
            {
                userWorksets = null;
            }
        }
        private WorksetDefaultVisibilitySettings worksetDefaultVisibilitySettings;
        public WorksetDefaultVisibilitySettings WorksetDefaultVisibilitySettings
        {
            get
            {
                if (worksetDefaultVisibilitySettings == null)
                {
                    worksetDefaultVisibilitySettings = new FilteredElementCollector(Document).OfClass(typeof(WorksetDefaultVisibilitySettings)).Cast<WorksetDefaultVisibilitySettings>().First();
                }
                return worksetDefaultVisibilitySettings;
            }
            set
            {
                worksetDefaultVisibilitySettings = value;
            }
        }
        private IEnumerable<Rebar> rebars;
        public IEnumerable<Rebar> Rebars
        {
            get
            {
                if (rebars == null)
                {
                    rebars = InstanceElements.Where(x => x is Rebar).Cast<Rebar>();
                }
                return rebars;
            }
            set
            {
                rebars = value;
            }
        }
        private IEnumerable<RebarHookType> rebarHookTypes;
        public virtual IEnumerable<RebarHookType> RebarHookTypes
        {
            get
            {
                if (rebarHookTypes == null)
                {
                    rebarHookTypes = TypeElements.Where(x => x is RebarHookType).Cast<RebarHookType>();
                }
                return rebarHookTypes;
            }
        }
        private IEnumerable<RebarBarType> rebarBarTypes;
        public IEnumerable<RebarBarType> RebarBarTypes
        {
            get
            {
                if (rebarBarTypes == null)
                {
                    rebarBarTypes = TypeElements.Where(x => x is RebarBarType).Cast<RebarBarType>();
                }
                return rebarBarTypes;
            }
            set
            {
                rebarBarTypes = value;
            }
        }
        private IEnumerable<RebarShape> rebarShapes;
        public IEnumerable<RebarShape> RebarShapes
        {
            get
            {
                if (rebarShapes == null)
                {
                    rebarShapes = TypeElements.Where(x => x is RebarShape).Cast<RebarShape>();
                }
                return rebarShapes;
            }
            set
            {
                rebarShapes = value;
            }
        }
        private List<Category> categories;
        public List<Category> Categories
        {
            get
            {
                if (categories == null)
                {
                    categories = new List<Category>();
                    foreach (Category cate in Document.Settings.Categories)
                    {
                        (categories as List<Category>).Add(cate);
                    }
                }
                return categories;
            }
            set
            {
                categories = value;
            }
        }
        private IEnumerable<TextNoteType> textNoteTypes;
        public IEnumerable<TextNoteType> TextNoteTypes
        {
            get
            {
                if (textNoteTypes == null)
                {
                    textNoteTypes = TypeElements.Where(x => x is TextNoteType).Cast<TextNoteType>();
                }
                return textNoteTypes;
            }
            set
            {
                textNoteTypes = value;
            }
        }
        private IEnumerable<Level> levels;
        public IEnumerable<Level> Levels
        {
            get
            {
                if (levels == null)
                {
                    levels = InstanceElements.Where(x => x is Level).Cast<Level>();
                }
                return levels;
            }
            set
            {
                levels = value;
            }
        }
        private IEnumerable<WallType> wallTypes;
        public IEnumerable<WallType> WallTypes
        {
            get
            {
                if (wallTypes == null)
                {
                    wallTypes = TypeElements.Where(x => x is WallType).Cast<WallType>();
                }
                return wallTypes;
            }
            set
            {
                wallTypes = value;
            }
        }
        private BindingMap parameterBindings;
        public BindingMap ParameterBindings
        {
            get
            {
                if (parameterBindings == null)
                {
                    parameterBindings = Document.ParameterBindings;
                }
                return parameterBindings;
            }
            set
            {
                parameterBindings = value;
            }
        }
        private IEnumerable<FillPatternElement> fillPatternElements;
        public IEnumerable<FillPatternElement> FillPatternElements
        {
            get
            {
                if (fillPatternElements == null)
                {
                    fillPatternElements = InstanceElements.Where(x => x is FillPatternElement)
                        .Cast<FillPatternElement>();
                }
                return fillPatternElements;
            }
            set
            {
                fillPatternElements = value;
            }
        }
        public UIView ActiveUIView
        {
            get
            {
                return UIDocument.GetOpenUIViews().Where(x => x.ViewId.IntegerValue == ActiveView.Id.IntegerValue).First();
            }
        }
        private IEnumerable<RebarCoverType> rebarCoverTypes;
        public IEnumerable<RebarCoverType> RebarCoverTypes
        {
            get
            {
                if (rebarCoverTypes == null)
                {
                    rebarCoverTypes = TypeElements.Where(x => x is RebarCoverType).Cast<RebarCoverType>();
                }
                return rebarCoverTypes;
            }
            set
            {
                rebarCoverTypes = value;
            }
        }
        private IEnumerable<ViewSchedule> viewSchedules;
        public IEnumerable<ViewSchedule> ViewSchedules
        {
            get
            {
                if (viewSchedules == null)
                {
                    viewSchedules = InstanceElements.Where(x => (x is ViewSchedule) && !(x as ViewSchedule).ViewSpecific).Cast<ViewSchedule>();
                }
                return viewSchedules;
            }
            set
            {
                viewSchedules = value;
            }
        }
        private ReinforcementSettings reinforcementSettings;
        public ReinforcementSettings ReinforcementSettings
        {
            get
            {
                if (reinforcementSettings == null)
                {
                    reinforcementSettings = InstanceElements
                        .Where(x => (x is ReinforcementSettings))
                        .Cast<ReinforcementSettings>().First();
                }
                return reinforcementSettings;
            }
            set
            {
                reinforcementSettings = value;
            }
        }
        private RebarRoundingManager rebarRoundingManager;
        public RebarRoundingManager RebarRoundingManager
        {
            get
            {
                if (rebarRoundingManager == null)
                {
                    rebarRoundingManager = ReinforcementSettings.GetRebarRoundingManager();
                }
                return rebarRoundingManager;
            }
            set
            {
                rebarRoundingManager = value;
            }
        }
        private IEnumerable<RevitLinkInstance> revitLinkInstances;
        public IEnumerable<RevitLinkInstance> RevitLinkInstances
        {
            get
            {
                if (revitLinkInstances == null)
                {
                    revitLinkInstances = InstanceElements.Where(x => x is RevitLinkInstance).Cast<RevitLinkInstance>();
                }
                return revitLinkInstances;
            }
            set
            {
                revitLinkInstances = value;
            }
        }
        private List<Document> linkDocuments;
        public List<Document> LinkDocuments
        {
            get
            {
                if (linkDocuments == null)
                {
                    linkDocuments = new List<Document>();
                    foreach (var instance in RevitLinkInstances)
                    {
                        var linkDoc = instance.GetLinkDocument();
                        if (linkDoc == null) continue;
                        if (linkDocuments.Where(x => x.PathName == linkDoc.PathName).Count() == 0)
                        {
                            linkDocuments.Add(linkDoc);
                        }
                    }
                }
                return linkDocuments;
            }
            set
            {
                linkDocuments = value;
            }
        }
        public IEnumerable<Element> allInstances;
        public IEnumerable<Element> AllInstances
        {
            get
            {
                if (allInstances == null)
                {
                    allInstances = InstanceElements;
                    foreach (var linkDocument in LinkDocuments)
                    {
                        var instances = new FilteredElementCollector(linkDocument)
                            .WhereElementIsNotElementType();
                        allInstances = allInstances.Union(instances);
                    }
                }
                return allInstances;
            }
            set
            {
                allInstances = value;
            }
        }
        private ProjectInfo projectInfo;
        public ProjectInfo ProjectInfo
        {
            get
            {
                if (projectInfo == null)
                {
                    projectInfo = Document.ProjectInformation;
                }
                return projectInfo;
            }
            set
            {
                projectInfo = value;
            }
        }
        private BindingMap bindingMap;
        public BindingMap BindingMap
        {
            get
            {
                if (bindingMap == null)
                {
                    bindingMap = Document.ParameterBindings;
                }
                return bindingMap;
            }
            set
            {
                bindingMap = value;
            }
        }
        private SketchPlane workPlane;
        public SketchPlane WorkPlane
        {
            get
            {
                if (workPlane == null)
                {
                    workPlane = ActiveView.SketchPlane;
                    if (workPlane == null)
                    {
                        workPlane = ActiveView.SketchPlane = ActiveSketchPlane;
                    }    
                }
                return workPlane;
            }
            set
            {
                workPlane = value;
            }
        }

        private FailureDefinitionRegistry failureDefinitionRegistry;
        public FailureDefinitionRegistry FailureDefinitionRegistry
        {
            get
            {
                if (failureDefinitionRegistry == null)
                    failureDefinitionRegistry = Application.GetFailureDefinitionRegistry();
                return failureDefinitionRegistry;
            }
        }

        public ExternalEvent ExternalEvent { get; set; }
        //public ExternalEventHandler ExternalEventHandler { get; set; } = new ExternalEventHandler();
    }
}
