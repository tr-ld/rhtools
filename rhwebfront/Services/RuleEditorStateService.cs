using rhdata.Rules;

namespace RHWebFront.Services;

public class RuleEditorStateService
{
    private readonly Dictionary<int, Rule> _pendingChanges = new();
    private readonly HashSet<int> _newRuleIds = new();
    
    public event Action StateChanged;

    public void TrackChange(int ruleId, Rule pendingRule)
    {
        _pendingChanges[ruleId] = pendingRule;
        StateChanged?.Invoke();
    }

    public IEnumerable<int> GetNewRuleIds() => _newRuleIds.ToList();
    public IReadOnlyCollection<int> GetChangedRuleIds() => _pendingChanges.Keys.ToList();
    public Rule GetPendingRule(int ruleId) => _pendingChanges.TryGetValue(ruleId, out var rule) ? rule : null;
    public bool HasPendingChanges(int ruleId) => _pendingChanges.ContainsKey(ruleId) || _newRuleIds.Contains(ruleId);
    public bool HasAnyPendingChanges() => _pendingChanges.Any() || _newRuleIds.Any();

    public bool IsNew(int ruleId) => _newRuleIds.Contains(ruleId);
    public void MarkAsNew(int ruleId)
    {
        _newRuleIds.Add(ruleId);
        StateChanged?.Invoke();
    }

    public void UndoChanges(int ruleId)
    {
        _pendingChanges.Remove(ruleId);
        StateChanged?.Invoke();
    }

    public void UndoAllChanges()
    {
        _pendingChanges.Clear();
        StateChanged?.Invoke();
    }

    public void RemoveRule(int ruleId)
    {
        _newRuleIds.Remove(ruleId);
        _pendingChanges.Remove(ruleId);
        StateChanged?.Invoke();
    }

    public void Clear()
    {
        _pendingChanges.Clear();
        _newRuleIds.Clear();
        StateChanged?.Invoke();
    }
}