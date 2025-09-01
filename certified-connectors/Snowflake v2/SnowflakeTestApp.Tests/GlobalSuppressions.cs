// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics.CodeAnalysis;

// Test-specific suppressions for common patterns in test projects
[assembly: SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Test projects may need to catch general exceptions for validation and cleanup purposes")]
[assembly: SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic", Justification = "Test methods should not be static for MSTest framework compatibility")]
[assembly: SuppressMessage("Microsoft.Naming", "CA1707:IdentifiersShouldNotContainUnderscores", Justification = "Test method names can contain underscores for readability and following Given_When_Then pattern")]
[assembly: SuppressMessage("Microsoft.Design", "CA1020:AvoidNamespacesWithFewTypes", Justification = "Test projects often have focused namespaces with few types per area")]
[assembly: SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly", Justification = "Mocking frameworks may require settable collection properties")]
[assembly: SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope", Justification = "Test framework handles disposal of test objects")]
[assembly: SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", Justification = "Local variables in tests may be used for assertion purposes")]
[assembly: SuppressMessage("Microsoft.Performance", "CA1806:DoNotIgnoreMethodResults", Justification = "Tests may call methods without using return values for setup purposes")] 