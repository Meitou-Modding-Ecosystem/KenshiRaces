# A Biped (3ds Max, Bip001) .glb -> one skeleton-reference .glb per clip, Bip001 joints renamed to the Mixamo roles the Motion Lab reads.
import json,struct,sys
MAP={'Pelvis':'Hips','Spine':'Spine','Spine1':'Spine1','Spine2':'Spine2','Neck':'Neck','Head':'Head',
 'L Thigh':'LeftUpLeg','L Calf':'LeftLeg','L Foot':'LeftFoot','L Toe0':'LeftToeBase',
 'R Thigh':'RightUpLeg','R Calf':'RightLeg','R Foot':'RightFoot','R Toe0':'RightToeBase',
 'L UpperArm':'LeftArm','L Forearm':'LeftForeArm','L Hand':'LeftHand','L Clavicle':'LeftShoulder',
 'R UpperArm':'RightArm','R Forearm':'RightForeArm','R Hand':'RightHand','R Clavicle':'RightShoulder'}
src,outdir=sys.argv[1],sys.argv[2]
b=open(src,'rb').read()
jl=struct.unpack('<I',b[12:16])[0]; j=json.loads(b[20:20+jl]); rest=b[20+jl:]
for n in j['nodes']:
    nm=n.get('name','')
    if nm.startswith('Bip001 ') and nm[7:] in MAP: n['name']='mixamorig:'+MAP[nm[7:]]
anims=j['animations']
for a in anims:
    k=dict(j); k['animations']=[a]
    s=json.dumps(k).encode(); s+=b' '*((4-len(s)%4)%4)
    body=struct.pack('<II',len(s),0x4E4F534A)+s+rest
    open(f"{outdir}/{a['name']}.glb",'wb').write(struct.pack('<III',0x46546C67,2,12+len(body))+body)
    print(a['name'])
