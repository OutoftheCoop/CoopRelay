using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Core.Publishing;
using Umbraco.Core.Services;

namespace CoopRelay.Relay
{
    public class CacheManager : ApplicationEventHandler
    {
        public CacheManager()
        {
            ContentService.Published += ContentPublished;
            ContentService.Moving += ContentMoving;
            ContentService.Trashing += ContentTrashing;
            MemberService.Saved += MemberSaved;
        }

        private void ContentTrashing(IContentService sender, Umbraco.Core.Events.MoveEventArgs<IContent> args)
        {
            foreach (var node in args.MoveInfoCollection)
            {
                CoopRelay.Relay.Content.ForceExpireGet(node.Entity.Id);
                CoopRelay.Relay.Content.ForceExpireGetChildren(node.Entity.ParentId);
                CoopRelay.Relay.Content.ForceExpireGetChildren(node.NewParentId);
            }
        }

        private void ContentMoving(IContentService sender, Umbraco.Core.Events.MoveEventArgs<IContent> args)
        {
            foreach (var node in args.MoveInfoCollection)
            {
                CoopRelay.Relay.Content.ForceExpireGet(node.Entity.Id);
                CoopRelay.Relay.Content.ForceExpireGetChildren(node.Entity.ParentId);
                CoopRelay.Relay.Content.ForceExpireGetChildren(node.NewParentId);
            }
        }

        private void ContentPublished(IPublishingStrategy sender, Umbraco.Core.Events.PublishEventArgs<IContent> args)
        {
            foreach (var node in args.PublishedEntities)
            {
                CoopRelay.Relay.Content.ForceExpireGet(node.Id);
                CoopRelay.Relay.Content.ForceExpireGetChildren(node.ParentId);
            }
        }

        private void MemberSaved(IMemberService sender, Umbraco.Core.Events.SaveEventArgs<IMember> args)
        {
            foreach (var member in args.SavedEntities)
            {
                CoopRelay.Relay.Member.ForceExpireGet(member.Id);
            }
        }
    }
}
