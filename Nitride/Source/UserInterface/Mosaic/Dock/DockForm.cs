/// ***************************************************************************
/// Nitride Shared Libraries and Utilities
/// Copyright 2001-2008, 2014-2021 Xu Li - me@xuli.us
/// 
/// Docking Multi-Panel Control
/// 
/// ***************************************************************************

using System;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Nitride
{
    [Flags]
    public enum DockTabType
    {
        None = 0,
        Center = 1,
        Side = 2,
        Bottom = 4,
        Top = 8,
    }

    //[DesignerCategory("Code")]
    public abstract class DockForm : TabItem
    {
        #region Ctor
        protected DockForm(string formName, bool enableUiUpdate = false) : base(formName)
        {
            Name = formName;

            if (enableUiUpdate)
            {
                AsyncUpdateUITask = new Task(() => AsyncUpdateUIWorker(), AsyncUpdateUITask_Cts.Token);
                AsyncUpdateUITask.Start();
            }
        }
        #endregion

        #region Components
        protected DockCanvas DockCanvas => DockContainer is DockContainer dc ? dc.DockCanvas : null;
        protected DockContainer DockContainer => HostContainer is DockContainer dc ? dc : null;

        // To be deloyed to the RibbonTab
        public RibbonTabItem RibbonTabItem { get; set; }

        public DockTabType Type { get; set; }

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            if (Parent is not null && !(Parent is DockContainer))
            {
                //throw new Exception("DockForm can only be exsiting in DockContainer / Parent: " + Parent.GetType().ToString());
                Console.WriteLine("DockForm can only be exsiting in DockContainer / Parent: " + Parent.GetType().ToString());
            }
            Coordinate();
        }
        protected override void OnGotFocus(EventArgs e)
        {
            base.OnGotFocus(e);
            DockCanvas.ActiveDockForm = this;
            if (Parent is DockContainer dc) DockCanvas.ActiveDockContainer = dc;
            if (DockCanvas is not null) DockCanvas.Invalidate(true);
        }

        public override void Close()
        {
            Console.Write(TabName + ": The Tab is closing");

            AsyncUpdateUITask_Cts.Cancel();
            while (AsyncUpdateUITask.Status == TaskStatus.Running) ;

            if (HostContainer is not null)
            {
                HostContainer.Remove(this);
            }

        
            /*
            Dispose();
            while (Disposing) ;*/
        }

        #endregion

        #region Update UI

        public virtual bool ReadyToShow { get => IsActive && m_ReadyToShow; set { m_ReadyToShow = value; } }

        protected bool m_ReadyToShow = false;

        public virtual void DataIsUpdated(IDataProvider _) => AsyncUpdateUI = true;

        public bool AsyncUpdateUI { get; set; } = false;

        
        public readonly Task AsyncUpdateUITask;

        public readonly CancellationTokenSource AsyncUpdateUITask_Cts = new();

        protected virtual void AsyncUpdateUIWorker()
        {
            while (AsyncUpdateUITask_Cts.IsContinue())
            {
                if (AsyncUpdateUI)
                {
                    try 
                    {
                        this?.Invoke(() =>
                        {
                            CoordinateLayout();
                            Invalidate(false);
                            AsyncUpdateUI = false;
                        });
                    }
                    catch(Exception e) 
                    {
                        Console.WriteLine("DockForm AsyncUpdateUIWorker(): " + e.Message);
                    }

  
                    // Thread.Sleep(2);
                }
                else
                    Thread.Sleep(5);
            }
        }

        #endregion Update UI

        #region Coordinate

        public virtual bool IsActive
        {
            get
            {
                if (Width < 1)
                    return false;
                else if (!(Parent is null) && !(HostContainer is null))
                {
                    if (HostContainer.ActiveTab != this) return false;
                }
                else if (Parent is null || HostContainer is null)
                {
                    return false;
                }
                return true;
            }
        }

        #endregion

        #region Docking

        #endregion

        #region Mouse
        protected override void OnMouseClick(MouseEventArgs e)
        {
            //Focus();
            //DockControl.Invalidate(true);
            DockCanvas.ActiveDockForm = this;
            base.OnMouseClick(e);
        }
        #endregion

        #region Paint

        public object GraphicsLockObject { get; } = new object();

        #endregion
    }
}
