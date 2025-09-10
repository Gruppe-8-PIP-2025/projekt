using System;

/// <summary>
/// Aggregates information about the current play-session.
/// </summary>
public class SessionStatistics
{
  private DateTime sessionStart;
  private DateTime sessionEnd;


  #region Constructor
  public SessionStatistics()
  {
    sessionStart = DateTime.Now;
  }
  #endregion
}