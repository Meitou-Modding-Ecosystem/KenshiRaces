# Export every ref_ clip of a Motion Lab rigged glb into one Kenshi .skeleton, named kr_<clip lower case>, played in place.
import os, sys, json, re, numpy as np
sys.path.insert(0, os.environ.get('MOTIONLAB', '../PZAKenshiMotionLab'))
from motionlab import kenshi_export as ke
from motionlab import mathx as mx
glb, skel, out, info_out = sys.argv[1:5]
got = ke.clip_locals(glb)
clips = sorted(c for c in got if c.startswith('ref_'))
anims, info = ke.build(glb, skel, clips=clips, prefix='', travel='remove')
renamed = []
for (name, length, tracks) in anims:
    kn = 'kr_' + re.sub(r'_+', '_', name[4:].lower()).strip('_')
    renamed.append((kn, length, tracks))
    info[name]['kenshi_name'] = kn
    T, Q = got[name][1]['Bip01']
    f = lambda q: float(np.degrees(np.arctan2(*(mx.q_to_m3(q) @ np.array([0, 0, 1.0]))[[0, 2]])))
    info[name]['yaw_start_end'] = [round(f(Q[0]), 1), round(f(Q[-1]), 1)]
n = ke.write_skeleton(open(skel, 'rb').read(), renamed, out)
json.dump({info[c]['kenshi_name']: {'source': c[4:], 'length_s': info[c]['length_s'], 'travel_m': info[c]['travel_m'], 'yaw': info[c]['yaw_start_end']} for c in clips}, open(info_out, 'w'), indent=1)
print(out, n, 'bytes', len(renamed), 'clips')
