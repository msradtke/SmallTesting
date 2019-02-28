using Prism.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmallTesting.RegionEvents
{
    public class RegionPubSubEvent<TPayload> : PubSubEvent<TPayload>
    {
  
        public override void Publish(TPayload payload)
        {
            var regionPayload = new RegionEventPayload<TPayload>();
            regionPayload.PublishIds = PublishIds;

            base.Publish(payload);
        }
        public List<Guid> PublishIds { get; set; }

    }
}
